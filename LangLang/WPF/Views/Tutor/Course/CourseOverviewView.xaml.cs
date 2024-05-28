using System.Collections.ObjectModel;
using System.Windows.Controls;
using LangLang.Domain.Model;
using LangLang.WPF.ViewModels.Director;
using LangLang.WPF.ViewModels.Tutor.Course;

namespace LangLang.WPF.Views.Tutor.Course
{
    public partial class CourseOverviewView : UserControl
    {
        public CourseOverviewView()
        {
            InitializeComponent();
        }
        private void cbSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = new ObservableCollection<WorkDay>();
            foreach (var item in cbSchedule.SelectedItems)
            {
                selectedItems.Add((WorkDay)item);
            }
            if (DataContext is CourseOverviewViewModel viewModel)
            {
                viewModel.ScheduleDays = selectedItems;
            } else if (DataContext is CourseOverviewForDirectorViewModel viewModel2)
            {
                viewModel2.ScheduleDays = selectedItems;
            }
        }

        private void dgCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DataContext is CourseOverviewViewModel || DataContext is CourseOverviewForDirectorViewModel)
            {
                cbSchedule.SelectedItems.Clear();
                cbMonday.SelectedIndex = -1;
                cbTuesday.SelectedIndex = -1;
                cbWednesday.SelectedIndex = -1;
                cbThursday.SelectedIndex = -1;
                cbFriday.SelectedIndex = -1;

                var selectedCourse = ((DataGrid)sender).SelectedItem as CourseViewModel;
                if(selectedCourse != null)
                {
                    foreach(WorkDay day in selectedCourse.Schedule.Schedule.Keys)
                    {
                        cbSchedule.SelectedItems.Add(day);
                        switch(day)
                        {
                            case WorkDay.Monday:
                                cbMonday.SelectedValue = selectedCourse.Schedule.Schedule[day].Item1;
                                break;
                            case WorkDay.Tuesday:
                                cbTuesday.SelectedValue = selectedCourse.Schedule.Schedule[day].Item1;
                                break;
                            case WorkDay.Wednesday:
                                cbWednesday.SelectedValue = selectedCourse.Schedule.Schedule[day].Item1;
                                break;
                            case WorkDay.Thursday:
                                cbThursday.SelectedValue = selectedCourse.Schedule.Schedule[day].Item1;
                                break;
                            case WorkDay.Friday:
                                cbFriday.SelectedValue = selectedCourse.Schedule.Schedule[day].Item1;
                                break;
                        }
                    }
                }
            }
        }
    }
}
