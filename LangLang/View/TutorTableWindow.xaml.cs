using Consts;
using LangLang.Model;
using LangLang.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LangLang.View
{
    public partial class TutorTableWindow : Window
    {
        private bool deletingRow = false;
        public TutorTableWindow()
        {
            InitializeComponent();
            DataContext = new TutorTableViewModel(this, knownLanguagesHolder!);
            dpBirthDate.DisplayDateStart = new DateTime(1924, 1, 1);
            dpBirthDate.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16
        }
        public void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (DataContext is not TutorTableViewModel viewModel) // unable to parse DataContext
                return;
            if (viewModel.selectingTutor)            // event called from changing tutor selection, ignore call
                return;
            if (deletingRow)                         // event called from deleting language row, ignore call
                return;
            if (sender == null)                      // sender parameter not sent correctly
                return;
            if (viewModel.KnownLanguages.Count == 0) // event called while initializing window, ignore call
                return;
            if (e.AddedItems.Count == 0 || e.AddedItems[0] == null) // if selection is canceled or invalid
                return;
            if (sender is not ComboBox || (sender as ComboBox)!.Parent == null)
                return;

            Grid row = ((sender as ComboBox)!.Parent as Grid)!;
            ItemsControl parent = (row.Parent as ItemsControl)!;
            int index = parent.Items.IndexOf(row);
            string languageName = (string)e.AddedItems[0]!;

            viewModel.ChangeLanguageCommand.Execute(Tuple.Create(index, languageName));
        }

        public void LevelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not TutorTableViewModel viewModel) // unable to parse DataContext
                return;
            if (viewModel.selectingTutor)            // event called from changing tutor selection, ignore call
                return;
            if (deletingRow)                         // event called from deleting language row, ignore call
                return;
            if (sender == null)                      // sender parameter not sent correctly
                return;
            if (viewModel.KnownLanguages.Count == 0) // event called while initializing window, ignore call
                return;
            if (e.AddedItems.Count == 0 || e.AddedItems[0] == null) // if selection is canceled or invalid
                return;
            if (sender is not ComboBox || (sender as ComboBox)!.Parent == null)
                return;

            Grid row = ((sender as ComboBox)!.Parent as Grid)!;
            ItemsControl parent = (row.Parent as ItemsControl)!;
            int index = parent.Items.IndexOf(row);
            LanguageLvl level = (LanguageLvl)e.AddedItems[0]!;

            viewModel.ChangeLevelCommand.Execute(Tuple.Create(index, level));
        }

        public void DeleteKnownLanguageClicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is not TutorTableViewModel viewModel)
                return;
            if (sender == null)
                return;
            deletingRow = true;
            Grid row = ((sender as Button)!.Parent as Grid)!;
            ItemsControl parent = (row.Parent as ItemsControl)!;
            int index = parent.Items.IndexOf(row);
            parent.Items.Remove(row);
            viewModel.DeleteKnownLanguageCommand.Execute(index);
            deletingRow = false;
        }

        public void AddKnownLanguage(int languageIndex, int levelIndex, ObservableCollection<string?> languages, ObservableCollection<LanguageLvl> levels)
        {
            int index = knownLanguagesHolder.Items.Count - 1;
            
            Grid newRow = new();
            newRow.ColumnDefinitions.Add(new ColumnDefinition());
            newRow.ColumnDefinitions.Add(new ColumnDefinition());
            newRow.ColumnDefinitions.Add(new ColumnDefinition());
            newRow.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            newRow.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            newRow.ColumnDefinitions[2].Width = new GridLength(20);

            ComboBox languagesCB = new()
            {
                Margin = new Thickness(0),
                Name = $"cbLanguages{index}",
                ItemsSource = languages,
                SelectedIndex = languageIndex
            };
            languagesCB.SetValue(Grid.ColumnProperty, 0);
            languagesCB.SelectionChanged += LanguageSelectionChanged;
            
            ComboBox levelsCB = new()
            {
                Margin = new Thickness(5, 0, 0, 0),
                Name = $"cbLevels{index}",
                ItemsSource = levels,
                SelectedIndex = levelIndex
            };
            levelsCB.SetValue(Grid.ColumnProperty, 1);
            levelsCB.SelectionChanged += LevelSelectionChanged;
            
            Button deleteB = new()
            {
                Margin = new Thickness(3, 0, 0, 3),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                FontSize = 10,
                Content = "╳"
            };
            deleteB.SetValue(Grid.ColumnProperty, 2);
            deleteB.Click += DeleteKnownLanguageClicked;

            newRow.Children.Add(languagesCB);
            newRow.Children.Add(levelsCB);
            newRow.Children.Add(deleteB);
            knownLanguagesHolder.Items.Insert(index, newRow);
        }
    }
}
