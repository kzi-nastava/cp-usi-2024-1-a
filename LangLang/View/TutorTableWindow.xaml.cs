using Consts;
using LangLang.Model;
using LangLang.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using LangLang.Services.AuthenticationServices;

namespace LangLang.View
{
    public partial class TutorTableWindow : Window
    {
        public TutorTableWindow()
        {
            InitializeComponent();
            ItemsControl? knownLanguagesHolder = FindName("knownLanguagesHolder") as ItemsControl;
            if (knownLanguagesHolder == null)
                throw new Exception("Known languages holder ItemsControl not found");
            // DataContext = new TutorTableViewModel(this, knownLanguagesHolder!, new RegisterService()); // ???
            dpBirthDate.DisplayDateStart = new DateTime(1924, 1, 1);
            dpBirthDate.DisplayDateEnd = DateTime.Today.AddYears(-16);   //minimum age of 16
        }
        public void cbLanguages_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            var viewModel = DataContext as TutorTableViewModel;
            if (viewModel == null)
                return;
            if (viewModel.selectingTutor)
                return;
            if (sender == null)
                return;
            if (viewModel.KnownLanguages == null || viewModel.KnownLanguages.Count == 0)
                return;
            if (e.AddedItems[0] == null)
                return;
            if (sender is not ComboBox || (sender as ComboBox)!.Parent == null)
                return;
            viewModel.changedLanguages = true;

            Grid row = ((sender as ComboBox)!.Parent as Grid)!;
            ItemsControl parent = (row.Parent as ItemsControl)!;

            int index = parent.Items.IndexOf(row);
            viewModel.KnownLanguages[index] = new ((e.AddedItems[0] as string)!, viewModel.KnownLanguages[index].Item2);
        }

        public void cbLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TutorTableViewModel;
            if (viewModel == null)
                return;
            if (viewModel.selectingTutor)
                return;
            if (sender == null)
                return;
            if (viewModel.KnownLanguages == null || viewModel.KnownLanguages.Count == 0)
                return;
            if (e.AddedItems[0] == null)
                return;
            if (sender is not ComboBox || (sender as ComboBox)!.Parent == null)
                return;
            viewModel.changedLanguages = true;

            Grid row = ((sender as ComboBox)!.Parent as Grid)!;
            ItemsControl parent = (row.Parent as ItemsControl)!;

            int index = parent.Items.IndexOf(row);
            viewModel.KnownLanguages[index] = new(viewModel.KnownLanguages[index].Item1, (LanguageLvl)e.AddedItems[0]!);
        }

        private void dgTutors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TutorTableViewModel;
            if(viewModel != null)
            {
            }
        }

        public void DeleteKnownLanguage_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TutorTableViewModel;
            if (viewModel == null)
                return;
            if (sender == null)
                return;
            viewModel.changedLanguages = true;
            Grid row = ((sender as Button)!.Parent as Grid)!;
            ItemsControl parent = (row.Parent as ItemsControl)!;
            int index = parent.Items.IndexOf(row);
            parent.Items.Remove(row);
            viewModel.KnownLanguages.RemoveAt(index);
        }
    }
}
