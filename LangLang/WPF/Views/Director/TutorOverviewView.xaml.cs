using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Director;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Director
{
    public partial class TutorOverviewView : UserControl
    {
        private bool deletingRow = false;
        public TutorOverviewView()
        {
            InitializeComponent();
            DataContextChanged += new DependencyPropertyChangedEventHandler(SubscribeToEvents);
            dpBirthDate.DisplayDateStart = new DateTime(1924, 1, 1);
            dpBirthDate.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16
        }
        public void SubscribeToEvents(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null) // not first call, avoiding double subriptions
                return;
            if (e.NewValue is not TutorOverviewViewModel viewModel) // invalid event arguments
                return;
            viewModel.RemoveKnownLanguages += RemoveKnownLanguages;
            viewModel.KnownLanguageAdded += AddKnownLanguage;
            viewModel.RefreshSelectionCommand.Execute(null);
        }

        public void LanguageSelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (DataContext is not TutorOverviewViewModel viewModel) // unable to parse DataContext
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
            if (DataContext is not TutorOverviewViewModel viewModel) // unable to parse DataContext
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
            LanguageLevel level = (LanguageLevel)e.AddedItems[0]!;

            viewModel.ChangeLevelCommand.Execute(Tuple.Create(index, level));
        }

        public void DeleteKnownLanguageClicked(object sender, RoutedEventArgs e)
        {
            if (DataContext is not TutorOverviewViewModel viewModel)
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

        public void AddKnownLanguage(int languageIndex, int levelIndex, ObservableCollection<string?> languages, ObservableCollection<LanguageLevel> levels)
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

        public void RemoveKnownLanguages()
        {
            while (knownLanguagesHolder.Items.Count > 1)
                knownLanguagesHolder.Items.RemoveAt(0); // remove everything except + button
        }

        
    }
}
