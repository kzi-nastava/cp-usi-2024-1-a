using System;
using System.Windows;
using System.Windows.Input;
using LangLang.Application.Stores;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.User;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Student
{
    public class RateTutorViewModel : ViewModelBase, INavigableDataContext
    {
        public NavigationStore NavigationStore { get; }

        private readonly Domain.Model.Student _loggedInUser;

        private readonly CurrentCourseStore _currentCourseStore;

        private readonly ICourseAttendanceService _courseAttendanceService;

        private readonly IAuthenticationStore _authenticationStore;

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

        public RateTutorViewModel(NavigationStore navigationStore, CurrentCourseStore currentCourseStore, ITutorService tutorService, IAuthenticationStore authenticationStore, ICourseAttendanceService courseAttendanceService)
        {
            _authenticationStore = authenticationStore;
            _loggedInUser = (Domain.Model.Student?)authenticationStore.CurrentUser.Person!;
            NavigationStore = navigationStore;
            _currentCourseStore = currentCourseStore;
            _courseAttendanceService = courseAttendanceService;
            _tutorService = tutorService;
            _selectedRating = 0;
            SubmitRatingCommand = new RelayCommand(SubmitRating!);
        }

        private void SubmitRating(object parameter)
        {
            int selectedRating = Convert.ToInt32(parameter);

            bool successful = _courseAttendanceService.RateTutor(_currentCourseStore.CurrentCourse!, _loggedInUser.Id, selectedRating);

            Domain.Model.Tutor tutor = _tutorService.GetTutorById(_currentCourseStore.CurrentCourse!.TutorId!)!;
            if (successful)
            {
                MessageBox.Show($"You've rated tutor {tutor.Name} {tutor.Surname} with {selectedRating}.", "Success");
            }
            else
            {
                MessageBox.Show($"You've already rated tutor {tutor.Name} {tutor.Surname}!", "Unsuccessful");

            }

        }

    }
}
