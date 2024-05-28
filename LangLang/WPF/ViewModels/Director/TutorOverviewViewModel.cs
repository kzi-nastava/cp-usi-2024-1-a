using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Authentication;
using LangLang.Application.UseCases.Common;
using LangLang.Application.UseCases.User;
using LangLang.Application.Utility.Authentication;
using LangLang.Application.Utility.Navigation;
using LangLang.Application.Utility.Validators;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.Views.Director;

namespace LangLang.WPF.ViewModels.Director
{
    public class TutorOverviewViewModel : ViewModelBase, INavigableDataContext
    {
        private readonly ITutorService _tutorService;
        private readonly ILanguageService _languageService;
        private readonly IAccountService _accountService;
        private readonly IUserValidator _userValidator;
        private readonly IUserProfileMapper _userProfileMapper;

        public NavigationStore NavigationStore { get; }

        public RelayCommand AddKnownLangaugeCommand { get; }
        public RelayCommand ChangeLanguageCommand { get; }
        public RelayCommand ChangeLevelCommand { get; }
        public RelayCommand DeleteKnownLanguageCommand { get; }
        public RelayCommand RefreshSelectionCommand { get; }
        public RelayCommand AddTutorCommand { get; }
        public RelayCommand DeleteTutorCommand { get; }
        public RelayCommand UpdateTutorCommand { get; }
        public RelayCommand ClearFiltersCommand { get; }
        public RelayCommand PreviousPageCommand { get; }
        public RelayCommand NextPageCommand { get; }

        public ObservableCollection<TutorOverviewDto> Tutors{ get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLevel> Levels { get; set; }
        public ObservableCollection<Gender> Genders { get; set; }

        public bool changedLanguages;
        public bool changedEmailOrPassword;
        public bool selectingTutor;

        public event Action? RemoveKnownLanguages;
        public event KnownLanguageAddedHandler? KnownLanguageAdded;
        public delegate void KnownLanguageAddedHandler(int languageIndex, int levelIndex, ObservableCollection<string?> languages, ObservableCollection<LanguageLevel> levels);

        private bool _filterIsActive;
        private int _pageNumber = 1;
        private int _tutorsPerPage = 5;

        private string _name = "";
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }
        private string _surname = "";
        public string Surname
        {
            get => _surname;
            set => SetField(ref _surname, value);
        }
        private string _email = "";
        public string Email
        {
            get => _email;
            set 
            {
                _email = value;
                changedEmailOrPassword = true;
                OnPropertyChanged();
            }
        }
        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                changedEmailOrPassword = true;
                OnPropertyChanged();
            }
        }
        private string _phoneNumber = "";
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetField(ref _phoneNumber, value);
        }
        private Gender _selectedGender;
        public Gender SelectedGender
        {
            get => _selectedGender;
            set => SetField(ref _selectedGender, value);
        }

        private DateTime? _birthDate = null;
        public DateTime? BirthDate
        {
            get => _birthDate;
            set => SetField(ref _birthDate, value);
        }
        private DateTime? _dateAdded = null;
        public DateTime? DateAdded
        {
            get => _dateAdded != null ? _dateAdded : DateTime.Now;
            set => SetField(ref _dateAdded, value);
        }
        private List<Tuple<string, LanguageLevel>> _knownLanguages = new();
        public List<Tuple<string, LanguageLevel>> KnownLanguages
        {
            get => _knownLanguages;
            set => SetField(ref _knownLanguages, value);
        }

        // Filter values
        private string _languageFilter = "";
        public string LanguageFilter
        {
            get => _languageFilter;
            set
            {
                _languageFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }

        private LanguageLevel? _levelFilter;
        public LanguageLevel? LevelFilter
        {
            get => _levelFilter;
            set
            {
                _levelFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }

        private DateTime? _dateAddedMinFilter;
        public DateTime? DateAddedMinFilter
        {
            get => _dateAddedMinFilter;
            set
            {
                _dateAddedMinFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }
        private DateTime? _dateAddedMaxFilter;
        public DateTime? DateAddedMaxFilter
        {
            get => _dateAddedMaxFilter;
            set
            {
                _dateAddedMaxFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }

        private TutorOverviewDto? _selectedItem;
        public TutorOverviewDto? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                changedLanguages = false;
                selectingTutor = true;
                if (_selectedItem != null)
                {
                    Domain.Model.Tutor? tutor = _tutorService.GetTutorById(_selectedItem.Id);
                    if (tutor == null) 
                        return;
                    UserDto userDto = new(tutor, UserType.Tutor);
                    Name = tutor.Name;
                    Surname = tutor.Surname;
                    Email = _userProfileMapper.GetProfile(userDto)!.Email;
                    Password = _userProfileMapper.GetProfile(userDto)!.Password;
                    PhoneNumber = tutor.PhoneNumber;
                    BirthDate = tutor.BirthDate;
                    DateAdded = tutor.DateAdded;
                    SelectedGender = tutor.Gender;
                    SwitchKnownLanguages(tutor);
                }
                changedEmailOrPassword = false;
                selectingTutor = false;
                OnPropertyChanged();
            }
        }

        public int PageNumber
        {
            get => _pageNumber;
            private set
            {
                SetField(ref _pageNumber, value);
                LoadTutors();
            }
        }

        public int TutorsPerPage
        {
            get => _tutorsPerPage;
            set
            {
                SetField(ref _tutorsPerPage, value);
                if (PageNumber == 1)
                    LoadTutors();
                else
                    PageNumber = 1;
            }
        }

        public ObservableCollection<int> PageSizeOptions { get; }

        public TutorOverviewViewModel(NavigationStore navigationStore, IAccountService accountService, ITutorService tutorService, ILanguageService languageService, IUserValidator userValidator, IUserProfileMapper userProfileMapper)
        {
            NavigationStore = navigationStore;
            _accountService = accountService;
            _tutorService = tutorService;
            _languageService = languageService;
            _userValidator = userValidator;
            _userProfileMapper = userProfileMapper;
            Tutors = new();
            Languages = new();
            Levels = new();
            Genders = new();
            LoadCollections();

            if (Languages.Count > 0)
                KnownLanguages.Add(new(Languages[0]!, Levels[0]));
            else
                throw new Exception("No languages found");

            PageSizeOptions = new ObservableCollection<int>() { 1, 5, 10, 20 };

            AddKnownLangaugeCommand = new RelayCommand(execute => AddKnownLanguage());
            ChangeLanguageCommand = new RelayCommand(execute => ChangeLanguage(execute as Tuple<int, string>));
            ChangeLevelCommand = new RelayCommand(execute => ChangeLevel(execute as Tuple<int, LanguageLevel>));
            DeleteKnownLanguageCommand = new RelayCommand(execute => DeleteKnownLangauge(execute as int?));
            AddTutorCommand = new RelayCommand(execute => AddTutor(), execute => CanAddTutor());
            DeleteTutorCommand = new RelayCommand(execute => DeleteTutor(), execute => CanDeleteTutor());
            UpdateTutorCommand = new RelayCommand(execute => UpdateTutor(), execute => CanUpdateTutor());
            ClearFiltersCommand = new RelayCommand(execute => ClearFilters());
            RefreshSelectionCommand = new RelayCommand(execute => RefreshSelection());
            PreviousPageCommand = new RelayCommand(_ => GoToPreviousPage(), _ => CanGoToPreviousPage());
            NextPageCommand = new RelayCommand(_ => GoToNextPage(), _ => CanGoToNextPage());
        }
        private void LoadCollections()
        {
            LoadTutors();
            LoadLanguages();
            LoadLanguageLevels();
            LoadGenders();
        }

        private void ClearFilters()
        {
            _filterIsActive = false;
            LanguageFilter = "";
            LevelFilter = null;
            DateAddedMinFilter = null;
            DateAddedMaxFilter = null;
            Tutors.Clear();
            LoadTutors();
            OnPropertyChanged();
        }

        private void AddKnownLanguage(Tuple<string, LanguageLevel>? value = null)
        {
            int languageIndex = 0, levelIndex = 0;
            if (value != null)
            {
                languageIndex = Languages.IndexOf(value.Item1);
                levelIndex = Levels.IndexOf(value.Item2);
            }
            KnownLanguageAdded?.Invoke(languageIndex, levelIndex, Languages, Levels);
            if (value == null)
            {
                changedLanguages = true;
                _knownLanguages.Add(Tuple.Create(Languages[0]!, LanguageLevel.A1));
            }
            else
                _knownLanguages.Add(value);
            OnPropertyChanged();
        }
        private void ChangeLanguage(Tuple<int, string>? indexLanguagePair)
        {
            if (indexLanguagePair == null)
                return;
            int index = indexLanguagePair.Item1;
            string languageName = indexLanguagePair.Item2;
            changedLanguages = true;
            KnownLanguages[index] = Tuple.Create(languageName, KnownLanguages[index].Item2); 
            OnPropertyChanged();
        }
        private void ChangeLevel(Tuple<int, LanguageLevel>? indexLevelPair)
        {
            if (indexLevelPair == null)
                return;
            int index = indexLevelPair.Item1;
            LanguageLevel level = indexLevelPair.Item2;
            changedLanguages = true;
            KnownLanguages[index] = Tuple.Create(KnownLanguages[index].Item1, level);
            OnPropertyChanged();
        }
        private void DeleteKnownLangauge(int? index)
        {
            if (index == null)
                return;
            changedLanguages = true;
            KnownLanguages.RemoveAt((int)index);
            OnPropertyChanged();
        }
        private void RefreshSelection() => SelectedItem = _selectedItem;
        private void SwitchKnownLanguages(Domain.Model.Tutor selectedTutor)
        {
            RemoveKnownLanguages?.Invoke();
            KnownLanguages.Clear();
            foreach (var tuple in selectedTutor.KnownLanguages)
                AddKnownLanguage(new Tuple<string, LanguageLevel>(tuple.Item1.Name, tuple.Item2));
        }
        private bool CanUpdateTutor()
        {
            if (SelectedItem != null)
            {
                if (changedLanguages || changedEmailOrPassword)
                    return false;
                return true;
            }
            return false;
        }

        private void UpdateTutor()
        {
            if (SelectedItem == null)
                return;

            bool errored = ErrorInvalidTutor(true);
            if (errored)
                return;

            List<Tuple<Language, LanguageLevel>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(_languageService.GetLanguageById(tuple.Item1)!, tuple.Item2));

            Domain.Model.Tutor tutor = _accountService.UpdateTutor(SelectedItem.Id, Password, Name, Surname, (DateTime)BirthDate!, SelectedGender, PhoneNumber, knownLanguagesRightType, (DateTime)DateAdded!);
            Tutors.Remove(SelectedItem);
            Tutors.Add(new TutorOverviewDto(tutor));
            RemoveInputs();
        }
        private void DeleteTutor()
        {
            if (SelectedItem == null) 
                return;
            Domain.Model.Tutor? tutor = _tutorService.GetTutorById(SelectedItem.Id);
            if (tutor == null)
                return;
            _accountService.DeleteTutor(tutor);
            Tutors.Remove(SelectedItem);
            RemoveInputs();
        }
        private bool CanDeleteTutor() => SelectedItem != null;
        private bool CanAddTutor() => true;
        private void AddTutor()
        {
            bool errored = ErrorInvalidTutor(false);
            if (errored) 
                return;

            List<Tuple<Language, LanguageLevel>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(_languageService.GetLanguageById(tuple.Item1)!, tuple.Item2));

            Domain.Model.Tutor tutor = _accountService.RegisterTutor(new RegisterTutorDto(
                Email,
                Password,
                Name,
                Surname,
                (DateTime)BirthDate!,
                SelectedGender,
                PhoneNumber,
                knownLanguagesRightType,
                (DateTime)DateAdded!
                ));
            Tutors.Add(new TutorOverviewDto(tutor));
            RemoveInputs();
            MessageBox.Show("The tutor was added successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /**<summary>
         *     Checks if tutor is invalid and shows error if so. Return true if an error is shown.
         * </summary> */
        private bool ErrorInvalidTutor(bool updating)
        {
            ValidationError error = ValidationError.None;
            error |= _userValidator.CheckUserData(Email, Password, Name, Surname, PhoneNumber, BirthDate);
            if (!updating)
                error |= _userValidator.EmailTaken(Email);
            if (KnownLanguages.Count != KnownLanguages.Select(tuple => tuple.Item1).Distinct().Count())
                error |= ValidationError.LangEntriesNotUnique;

            if (error == ValidationError.None)
                return false;

            List<string> messages = error.GetAllMessages();
            string combinedMessage = string.Join(",\n", messages);
            MessageBox.Show(combinedMessage, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            return true;
        }
        private void LoadTutors()
        {
            Tutors.Clear();
            List<Domain.Model.Tutor> tutors;
            if (_filterIsActive)
                tutors = _tutorService.GetFilteredTutorsForPage(PageNumber, TutorsPerPage, LanguageFilter,
                                                                            LevelFilter, DateAddedMinFilter,
                                                                            DateAddedMaxFilter);
            else
                tutors = _tutorService.GetAllTutorsForPage(PageNumber, TutorsPerPage);
            foreach (Domain.Model.Tutor tutor in tutors)
                Tutors.Add(new TutorOverviewDto(tutor));
        }
        private void LoadLanguages()
        {
            var languages = _languageService.GetAll();
            foreach (Language language in languages)
                Languages.Add(language.Name);
        }
        private void LoadLanguageLevels()
        {
            foreach (LanguageLevel lvl in Enum.GetValues(typeof(LanguageLevel)))
                Levels.Add(lvl);
        }
        private void LoadGenders()
        {
            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                Genders.Add(gender);
        }
        private void RemoveInputs()
        {
            Name = "";
            Surname = "";
            Email = "";
            Password = "";
            PhoneNumber = "";
            BirthDate = null;
            DateAdded = null;
            _selectedItem = null;

            selectingTutor = true;
            RemoveKnownLanguages?.Invoke(); 
            KnownLanguages.Clear();
            selectingTutor = false;
        }
        private void FilterTutors()
        {
            _filterIsActive = true;
            LoadTutors();
        }
        private void GoToPreviousPage()
            => PageNumber--;
        private void GoToNextPage()
            => PageNumber++;
        private bool CanGoToPreviousPage()
            => PageNumber > 1;
        private bool CanGoToNextPage()
            => Tutors.Count == TutorsPerPage;
    }
}
