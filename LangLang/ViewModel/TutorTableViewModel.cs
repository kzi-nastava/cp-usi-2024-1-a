using Consts;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services;
using LangLang.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LangLang.ViewModel
{
    internal class TutorTableViewModel : ViewModelBase
    {
        private readonly TutorTableWindow window;
        private readonly ItemsControl knownLanguagesHolder;
        private readonly TutorService tutorService = TutorService.GetInstance();
        private readonly LanguageService languageService = new();
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
            set => SetField(ref email, value);
        }
        private string password = "";
        public string Password
        {
            get => password;
            set => SetField(ref password, value);
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
            get => dateAdded;
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
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                changedLanguages = false;
                selectingTutor = true;
                if (selectedItem != null)
                {
                    Name = selectedItem.Name;
                    Surname = selectedItem.Surname;
                    Email = selectedItem.Email;
                    Password = selectedItem.Password;
                    PhoneNumber = selectedItem.PhoneNumber;
                    BirthDate = selectedItem.BirthDate;
                    DateAdded = selectedItem.DateAdded;
                    SelectedGender = selectedItem.Gender;
                    SwitchKnownLanguages(selectedItem);
                }
                selectingTutor = false;
                OnPropertyChanged();
            }
        }


        public TutorTableViewModel(TutorTableWindow window, ItemsControl knownLanguagesHolder)
        {
            this.window = window;
            this.knownLanguagesHolder = knownLanguagesHolder;

            Tutors = new();
            Languages = new();
            Levels = new();
            Genders = new();
            LoadCollections();

            if (Languages.Count > 0)
                KnownLanguages.Add(new(Languages[0]!, Levels[0]));
            else
                knownLanguagesHolder.Items.RemoveAt(0);

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
            DirectorWindow directorWindow = new();
            directorWindow.Show();
            window.Close();
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
            window.AddKnownLanguage(languageIndex, levelIndex, Languages, Levels);
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
            while (knownLanguagesHolder.Items.Count > 1)
                knownLanguagesHolder.Items.RemoveAt(0);
            KnownLanguages.Clear();
            foreach (var tuple in selectedItem.KnownLanguages)
                AddKnownLanguage(new Tuple<string, LanguageLvl>(tuple.Item1.Name, tuple.Item2));
        }
        private bool CanUpdateTutor()
        {
            if (SelectedItem != null)
            {
                if (changedLanguages)
                    return false;
                return true;
            }
            return false;
        }

        private string GenerateErrorMessage()
        {
            if (Email == null || Password == null || Name == null || Surname == null || PhoneNumber == null || Email == "" || Name == "" || Surname == "" || Password == "" || PhoneNumber == "")
                return "All the fields are required!";

            try
            {
                _ = new MailAddress(Email);
            }
            catch
            {
                return "Incorrect email!";
            }

            if (int.TryParse(Name, out _))
            {
                return "Name must be all letters!";
            }

            if (int.TryParse(Surname, out _))
            {
                return "Surname must be all letters!";
            }

            if (!int.TryParse(PhoneNumber, out _))
            {
                return "Phone number must be made up of numbers!";
            }
            if (Password.Length < 8 || !Password.Any(char.IsDigit) || !Password.Any(char.IsUpper))
            {
                return "Password must be at least 8 chars, uppercase and number!";
            }
            return "Invalid user data!";
        }

        private Tutor CreateTutor()
        {
            List<Tuple<Language, LanguageLvl>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(languageService.GetLanguageById(tuple.Item1), tuple.Item2));
            return new Tutor(Email, Password, Name, Surname, (DateTime)BirthDate!, SelectedGender, PhoneNumber, knownLanguagesRightType, new(), new(), new int[5], DateAdded);
        }

        private void UpdateTutor()
        {
            if (SelectedItem == null)
                return;

            bool errored = ErrorInvalidTutor();
            if (errored)
                return;

            Tutor tutor = CreateTutor();
            if (Email != SelectedItem.Email)
            {
                tutor.Email = SelectedItem.Email;
                tutorService.UpdateTutorEmail(tutor, Email);
            }
            tutorService.UpdateTutor(tutor);
            Tutors.Remove(SelectedItem);
            Tutors.Add(tutor);
            RemoveInputs();
        }
        private void DeleteTutor()
        {
            if (SelectedItem == null) 
                return;
            tutorService.DeleteTutor(SelectedItem.Email);
            Tutors.Remove(SelectedItem);
            RemoveInputs();
        }
        private bool CanDeleteTutor() => SelectedItem != null;
        private bool CanAddTutor() => true;
        private void AddTutor()
        {
            bool errored = ErrorInvalidTutor();
            if (errored) 
                return;

            Tutor tutor = CreateTutor();
            tutorService.AddTutor(tutor);
            Tutors.Add(tutor);
            RemoveInputs();
            MessageBox.Show("The tutor is added successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /**<summary>
         *     Checks if tutor is invalid and shows error if so. Return true if an error is shown.
         * </summary> */
        private bool ErrorInvalidTutor()
        {
            if (KnownLanguages.Count != KnownLanguages.Select(tuple => tuple.Item1).Distinct().Count())
            {
                MessageBox.Show("Languages entries must unique!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }

            bool valid = RegisterService.CheckUserData(Email, Password, Name, Surname, PhoneNumber);
            if (!valid)
            {
                MessageBox.Show(GenerateErrorMessage(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            if (RegisterService.CheckExistingEmail(Email))
            {
                MessageBox.Show("Email not avaliable!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            if (BirthDate == null)
            {
                MessageBox.Show("Birth date must be selected!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }
        public void LoadTutors()
        {
            var tutors = tutorService.GetAllTutors();
            foreach(Tutor tutor in tutors.Values)
                Tutors.Add(tutor);
        }
        public void LoadLanguages()
        {
            var languages = languageService.GetAll();
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
            while (knownLanguagesHolder.Items.Count > 1)
                knownLanguagesHolder.Items.RemoveAt(0); // remove everything except + button
            KnownLanguages.Clear();
            selectingTutor = false;
        }
        public void FilterTutors()
        {
            Tutors.Clear();
            var tutors = tutorService.GetAllTutors();
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
