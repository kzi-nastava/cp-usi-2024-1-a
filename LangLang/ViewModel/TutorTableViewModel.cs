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
using System.Windows.Controls;
using System.Windows.Media;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.UtilityServices;
using LangLang.Services.UserServices;
using LangLang.Services.UtilityServices;

namespace LangLang.ViewModel
{
    internal class TutorTableViewModel : ViewModelBase
    {
        private readonly TutorTableWindow window;
        private readonly ItemsControl knownLanguagesHolder;
        private readonly TutorService tutorService;
        private readonly LanguageService languageService;
        private readonly IRegisterService _registerService;
        public RelayCommand GoBackCommand { get; set; }
        public RelayCommand AddKnownLangaugeCommand { get; set; }
        public RelayCommand AddTutorCommand { get; set; }
        public RelayCommand DeleteTutorCommand { get; set; }
        public RelayCommand UpdateTutorCommand { get; set; }
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

        // FILTER VALUES
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


        public TutorTableViewModel(TutorTableWindow window, ItemsControl knownLanguagesHolder, IRegisterService registerService)
        {
            this.window = window;
            this.knownLanguagesHolder = knownLanguagesHolder;
            _registerService = registerService;
            Tutors = new();
            Languages = new();
            Levels = new();
            Genders = new();
            LoadTutors();
            LoadLanguages();
            LoadLanguageLevels();
            if (Languages.Count > 0)
                KnownLanguages.Add(new(Languages[0]!, Levels[0]));
            else
                knownLanguagesHolder.Items.RemoveAt(0);
            LoadGenders();
            GoBackCommand = new RelayCommand(execute => GoBack());
            AddKnownLangaugeCommand = new RelayCommand(execute => AddKnownLanguage());
            AddTutorCommand = new RelayCommand(execute => AddTutor(), execute => CanAddTutor());
            DeleteTutorCommand = new RelayCommand(execute => DeleteTutor(), execute => CanDeleteTutor());
            UpdateTutorCommand = new RelayCommand(execute => UpdateTutor(), execute => CanUpdateTutor());
            ClearFiltersCommand = new RelayCommand(execute => ClearFilters());
        }

        private void GoBack()
        {
            // DirectorWindow directorWindow = new();
            // directorWindow.Show();
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
            if (value == null)
                changedLanguages = true;
            int index = knownLanguagesHolder.Items.Count - 1;
            Grid newRow = new Grid();
            newRow.ColumnDefinitions.Add(new ColumnDefinition());
            newRow.ColumnDefinitions.Add(new ColumnDefinition());
            newRow.ColumnDefinitions.Add(new ColumnDefinition());
            newRow.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            newRow.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            newRow.ColumnDefinitions[2].Width = new GridLength(20);
            ComboBox languagesCB = new ComboBox();
            languagesCB.Margin = new Thickness(0);
            languagesCB.SetValue(Grid.ColumnProperty, 0);
            languagesCB.Name = $"cbLanguages{index}";
            languagesCB.SelectionChanged += window.cbLanguages_SelectionChanged;
            languagesCB.ItemsSource = Languages;
            languagesCB.SelectedIndex = 0;
            ComboBox levelsCB = new ComboBox();
            levelsCB.Margin = new Thickness(5,0,0,0);
            levelsCB.SetValue(Grid.ColumnProperty, 1);
            levelsCB.Name = $"cbLevels{index}";
            levelsCB.SelectionChanged += window.cbLevel_SelectionChanged;
            levelsCB.ItemsSource = Levels;
            levelsCB.SelectedIndex = 0;
            Button deleteB = new Button();
            deleteB.Margin = new Thickness(3, 0, 0, 3);
            deleteB.SetValue(Grid.ColumnProperty, 2);
            deleteB.Background = Brushes.Transparent;
            deleteB.BorderThickness = new Thickness(0);
            deleteB.FontSize = 10;
            deleteB.Click += window.DeleteKnownLanguage_Click;
            deleteB.Content = "╳";
            
            newRow.Children.Add(languagesCB);
            newRow.Children.Add(levelsCB);
            newRow.Children.Add(deleteB);
            knownLanguagesHolder.Items.Insert(index, newRow);

            if (value == null)
                knownLanguages.Insert(index, new Tuple<string, LanguageLvl>(Languages[0]!, LanguageLvl.A1));
            else
            {
                knownLanguages.Insert(index, value);
                languagesCB.SelectedIndex = Languages.IndexOf(value.Item1);
                levelsCB.SelectedIndex = Levels.IndexOf(value.Item2);
            }
            OnPropertyChanged();
        }

        private void SwitchKnownLanguages(Tutor selectedItem)
        {
            while (knownLanguagesHolder.Items.Count > 1)
            {
                knownLanguagesHolder.Items.RemoveAt(0);
            }
            KnownLanguages.Clear();
            foreach (var tuple in selectedItem.KnownLanguages)
            {
                AddKnownLanguage(new Tuple<string, LanguageLvl>(tuple.Item1.Name, tuple.Item2));
            }
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

        private void UpdateTutor()
        {
            if (SelectedItem == null)
                return;

            bool valid = _registerService.CheckUserData(Email, Password, Name, Surname, PhoneNumber);
            if (!valid)
            {
                MessageBox.Show(GenerateErrorMessage(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Tutor? tutorWithThisEmail = tutorService.GetTutor(Email);
            if (tutorWithThisEmail != null && tutorWithThisEmail != SelectedItem)
            {
                MessageBox.Show("Email not avaliable!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (BirthDate == null)
            {
                MessageBox.Show("Birth date must be selected!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<Tuple<Language, LanguageLvl>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(languageService.GetLanguageById(tuple.Item1), tuple.Item2));
            Tutor tutor = new Tutor(SelectedItem.Email, Password, Name, Surname, (DateTime)BirthDate, SelectedGender, PhoneNumber, knownLanguagesRightType, new(), new(), new int[5], DateAdded);
            tutorService.UpdateTutor(tutor);
            if (Email != SelectedItem.Email)
            {
                tutorService.UpdateTutorEmail(tutor, Email);
            }
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
        private bool CanDeleteTutor()
        {
            return SelectedItem != null;
        }
        private bool CanAddTutor()
        {
            return true;
        }
        private void AddTutor()
        {
            if (KnownLanguages.Count != KnownLanguages.Select(tuple => tuple.Item1).Distinct().Count())
            {
                MessageBox.Show("Languages entries must unique!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool valid = _registerService.CheckUserData(Email, Password, Name, Surname, PhoneNumber);
            if (!valid)
            {
                MessageBox.Show(GenerateErrorMessage(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_registerService.CheckExistingEmail(Email))
            {
                MessageBox.Show("Email not avaliable!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (BirthDate == null)
            {
                MessageBox.Show("Birth date must be selected!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<Tuple<Language, LanguageLvl>> knownLanguagesRightType = new();
            foreach (var tuple in KnownLanguages)
                knownLanguagesRightType.Add(new(languageService.GetLanguageById(tuple.Item1), tuple.Item2));
            Tutor tutor = new Tutor(Email, Password, Name, Surname, (DateTime)BirthDate, SelectedGender, PhoneNumber, knownLanguagesRightType, new(), new(), new int[5], DateAdded);
            tutorService.AddTutor(tutor);
            Tutors.Add(tutor);
            RemoveInputs();
            MessageBox.Show("The tutor is added successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void LoadTutors()
        {
            var tutors = tutorService.GetAllTutors();
            foreach(Tutor tutor in tutors.Values){
                Tutors.Add(tutor);
            }
        }
        public void LoadLanguages()
        {
            var languages = languageService.GetAll();
            foreach (Language language in languages)
            {
                Languages.Add(language.Name);
            }
        }
        public void LoadLanguageLevels()
        {
            foreach (LanguageLvl lvl in Enum.GetValues(typeof(LanguageLvl)))
            {
                Levels.Add(lvl);
            }
        }
        public void LoadGenders()
        {
            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
            {
                Genders.Add(gender);
            }
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
                knownLanguagesHolder.Items.RemoveAt(0);
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
