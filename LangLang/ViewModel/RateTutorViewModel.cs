using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.CourseServices;
using LangLang.Services.UserServices;
using LangLang.Stores;
using System;
using System.Windows;
using System.Windows.Input;


namespace LangLang.ViewModel
{
    public class RateTutorViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }

        private readonly CurrentCourseStore _currentCourseStore;

        private readonly ITutorService _tutorService;

        private int _selectedRating;
        public ICommand SubmitRatingCommand { get; }

        public int SelectedRating
        {
            get { return _selectedRating; }
            set
            {
                if (_selectedRating != value)
                {
                    _selectedRating = value;
                    OnPropertyChanged(nameof(SelectedRating));
                }
            }
        }

        public RateTutorViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, ITutorService tutorService)
        {
            NavigationStore = navigationStore;
            _currentCourseStore = currentCourseStore;
            _tutorService = tutorService;
            _selectedRating = 0;
            SubmitRatingCommand = new RelayCommand(SubmitRating!);
        }

        private void SubmitRating(object parameter)
        {
            int selectedRating = Convert.ToInt32(parameter);
            Tutor tutor = _tutorService.GetTutorForCourse(_currentCourseStore.CurrentCourse!.Id)!;
            _tutorService.AddRating(tutor, selectedRating);

            MessageBox.Show($"You've rated tutor {tutor.Name} {tutor.Surname} with {selectedRating}.", "Success");

        }

    }
}
