using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Media;
using LangLang.DTO;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.UtilityServices;
using LangLang.Services.UserServices;
using LangLang.Stores;
using LangLang.Services.NavigationServices;

namespace LangLang.ViewModel
{
    public class TutorTableViewModel : ViewModelBase, INavigableDataContext
    {
        private readonly ITutorService _tutorService;
        private readonly ILanguageService _languageService;
        private readonly IRegisterService _registerService;
        private readonly IAccountService _accountService;
        private readonly IUserValidator _userValidator;
        private readonly INavigationService _navigationService;
        private readonly IUserProfileMapper _userProfileMapper;
        public RelayCommand GoBackCommand { get; }
        public RelayCommand AddKnownLangaugeCommand { get; }
        public RelayCommand ChangeLanguageCommand { get; }
        public RelayCommand ChangeLevelCommand { get; }
        public RelayCommand DeleteKnownLanguageCommand { get; }
        public RelayCommand AddTutorCommand { get; }
        public RelayCommand DeleteTutorCommand { get; }
        public RelayCommand UpdateTutorCommand { get; }
        public RelayCommand ClearFiltersCommand { get; }
        public ObservableCollection<Tutor> Tutors{ get; set; }
        public ObservableCollection<string?> Languages { get; set; }
        public ObservableCollection<LanguageLvl> Levels { get; set; }
        public ObservableCollection<Gender> Genders { get; set; }
        public bool changedLanguages;
        public bool changedEmailOrPassword;
        public bool selectingTutor;

        private string name = "";
        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }
        private string surname = "";
        public string Surname
        {
            get => surname;
            set => SetField(ref surname, value);
        }
        private string email = "";
        public string Email
        {
            get => email;
            set 
            {
                email = value;
                changedEmailOrPassword = true;
                OnPropertyChanged();
            }
        }
        private string password = "";
        public string Password
        {
            get => password;
            set
            {
                password = value;
                changedEmailOrPassword = true;
                OnPropertyChanged();
            }
        }
        private string phoneNumber = "";
        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetField(ref phoneNumber, value);
        }
        private Gender selectedGender;
        public Gender SelectedGender
        {
            get => selectedGender;
            set => SetField(ref selectedGender, value);
        }

        private DateTime? birthDate = null;
        public DateTime? BirthDate
        {
            get => birthDate;
            set => SetField(ref birthDate, value);
        }
        private DateTime? dateAdded = null;
        public DateTime? DateAdded
        {
            get => dateAdded != null ? dateAdded : DateTime.Now;
            set => SetField(ref dateAdded, value);
        }
        private List<Tuple<string, LanguageLvl>> knownLanguages = new();
        public List<Tuple<string, LanguageLvl>> KnownLanguages
        {
            get => knownLanguages;
            set => SetField(ref knownLanguages, value);
        }

        // Filter values
        private string languageFilter = "";
        public string LanguageFilter
        {
            get => languageFilter;
            set
            {
                languageFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }

        private LanguageLvl? levelFilter;
        public LanguageLvl? LevelFilter
        {
            get => levelFilter;
            set
            {
                levelFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }

        private DateTime? dateAddedMinFilter;
        public DateTime? DateAddedMinFilter
        {
            get => dateAddedMinFilter;
            set
            {
                dateAddedMinFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }
        private DateTime? dateAddedMaxFilter;
        public DateTime? DateAddedMaxFilter
        {
            get => dateAddedMaxFilter;
            set
            {
                dateAddedMaxFilter = value;
                FilterTutors();
                OnPropertyChanged();
            }
        }

        private Tutor? selectedItem;
        public Tutor? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                changedLanguages = false;
                selectingTutor = true;
                if (selectedItem != null)
                {
                    UserDto userDto = new(selectedItem, UserType.Tutor);
                    Name = selectedItem.Name;
                    Surname = selectedItem.Surname;
                    Email = _userProfileMapper.GetProfile(userDto)!.Email;
                    Password = _userProfileMapper.GetProfile(userDto)!.Password;
                    PhoneNumber = selectedItem.PhoneNumber;
                    BirthDate = selectedItem.BirthDate;
                    DateAdded = selectedItem.DateAdded;
                    SelectedGender = selectedItem.Gender;
                    SwitchKnownLanguages(selectedItem);
                }
                changedEmailOrPassword = false;
                selectingTutor = false;
                OnPropertyChanged();
            }
        }
        private TutorTableWindow? _window;
        public TutorTableWindow Window
        {
            get => _window!;
            set => _window = value;
        }

        public NavigationStore NavigationStore { get; }

        public TutorTableViewModel(NavigationStore navigationStore, INavigationService navigationService, IRegisterService registerService, IAccountService accountService, ITutorService tutorService, ILanguageService languageService, IUserValidator userValidator, IUserProfileMapper userProfileMapper)
        {
            NavigationStore = navigationStore;
            _navigationService = navigationService;
            _registerService = registerService;
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

            GoBackCommand = new RelayCommand(execute => GoBack());
            AddKnownLangaugeCommand = new RelayCommand(execute => AddKnownLanguage());
            ChangeLanguageCommand = new RelayCommand(execute => ChangeLanguage(execute as Tuple<int, string>));
            ChangeLevelCommand = new RelayCommand(execute => ChangeLevel(execute as Tuple<int, LanguageLvl>));
            DeleteKnownLanguageCommand = new RelayCommand(execute => DeleteKnownLangauge(execute as int?));
            AddTutorCommand = new RelayCommand(execute => AddTutor(), execute => CanAddTutor());
            DeleteTutorCommand = new RelayCommand(execute => DeleteTutor(), execute => CanDeleteTutor());
            UpdateTutorCommand = new RelayCommand(execute => UpdateTutor(), execute => CanUpdateTutor());
            ClearFiltersCommand = new RelayCommand(execute => ClearFilters());
        }
        private void LoadCollections()
        {
            LoadTutors();
            LoadLanguages();
            LoadLanguageLevels();
            LoadGenders();
        }

        private void GoBack()
        {
            _navigationService.Navigate(Factories.ViewType.Director);
        }

        private void ClearFilters()
        {
            LanguageFilter = "";
            LevelFilter = null;
            DateAddedMinFilter = null;
            DateAddedMaxFilter = null;
            Tutors.Clear();
            LoadTutors();
            OnPropertyChanged();
        }

        private void AddKnownLanguage(Tuple<string, LanguageLvl>? value = null)
        {
            int languageIndex = 0, levelIndex = 0;
            if (value != null)
            {
                languageIndex = Languages.IndexOf(value.Item1);
                levelIndex = Levels.IndexOf(value.Item2);
            }
            Window.AddKnownLanguage(languageIndex, levelIndex, Languages, Levels);
            if (value == null)
            {
                changedLanguages = true;
                knownLanguages.Add(Tuple.Create(Languages[0]!, LanguageLvl.A1));
            }
            else
                knownLanguages.Add(value);
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
        private void ChangeLevel(Tuple<int, LanguageLvl>? indexLevelPair)
        {
            if (indexLevelPair == null)
                return;
            int index = indexLevelPair.Item1;
            LanguageLvl level = indexLevelPair.Item2;
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

        private void SwitchKnownLanguages(Tutor selectedItem)
        {
            Window.RemoveKnownLanguages();
            KnownLanguages.Clear();
            foreach (var tuple in selectedItem.KnownLanguages)
                AddKnownLanguage(new Tuple<string, LanguageLvl>(tuple.Item1.Name, tuple.Item2));
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

            List<Tuple<Language, LanguageLvl>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(_languageService.GetLanguageById(tuple.Item1)!, tuple.Item2));

            Tutor tutor = _accountService.UpdateTutor(SelectedItem.Id, Password, Name, Surname, (DateTime)BirthDate!, SelectedGender, PhoneNumber, knownLanguagesRightType, (DateTime)DateAdded!);
            Tutors.Remove(SelectedItem);
            Tutors.Add(tutor);
            RemoveInputs();
        }
        private void DeleteTutor()
        {
            if (SelectedItem == null) 
                return;
            _accountService.DeleteTutor(SelectedItem);
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

            List<Tuple<Language, LanguageLvl>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(_languageService.GetLanguageById(tuple.Item1)!, tuple.Item2));

            Tutor tutor = _accountService.RegisterTutor(new RegisterTutorDto(
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
            Tutors.Add(tutor);
            RemoveInputs();
            MessageBox.Show("The tutor is added successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
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
        public void LoadTutors()
        {
            var tutors = _tutorService.GetAllTutors();
            foreach(Tutor tutor in tutors.Values)
                Tutors.Add(tutor);
        }
        public void LoadLanguages()
        {
            var languages = _languageService.GetAll();
            foreach (Language language in languages)
                Languages.Add(language.Name);
        }
        public void LoadLanguageLevels()
        {
            foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
                Levels.Add(lvl);
        }
        public void LoadGenders()
        {
            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                Genders.Add(gender);
        }
        public void RemoveInputs()
        {
            Name = "";
            Surname = "";
            Email = "";
            Password = "";
            PhoneNumber = "";
            BirthDate = null;
            DateAdded = null;
            selectedItem = null;

            selectingTutor = true;
            Window.RemoveKnownLanguages(); 
            KnownLanguages.Clear();
            selectingTutor = false;
        }
        public void FilterTutors()
        {
            Tutors.Clear();
            var tutors = _tutorService.GetAllTutors();
            foreach (Tutor tutor in tutors.Values)
            {
                if (LanguageFilter != ""
                  && !tutor.KnownLanguages.Exists(tuple => tuple.Item1.Name == LanguageFilter))
                    continue;
                if (levelFilter != null
                  && !tutor.KnownLanguages.Exists(tuple => tuple.Item2 == levelFilter))
                    continue;
                if (DateAddedMinFilter != null && tutor.DateAdded < DateAddedMinFilter)
                    continue;
                if (DateAddedMaxFilter != null && tutor.DateAdded > DateAddedMaxFilter)
                    continue;
                Tutors.Add(tutor);
            }
        }
    }
}
