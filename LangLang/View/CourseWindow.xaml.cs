using Consts;
using LangLang.Model;
using LangLang.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LangLang.View
{
    public partial class CourseWindow : Window
    {
        public CourseWindow()
        {
            InitializeComponent();
            DataContext = new CourseViewModel(this);
           


        }
        private void cbSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as CourseViewModel;
            if (viewModel != null)
            {
                var selectedItems = new ObservableCollection<Consts.WorkDay>();
                foreach (var item in cbSchedule.SelectedItems)
                {
                    selectedItems.Add((Consts.WorkDay)item);
                }
                viewModel.ScheduleDays = selectedItems;
            }
        }

        private void dgCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as CourseViewModel;
            if(viewModel != null)
            {
                cbSchedule.SelectedItems.Clear();
                cbMonday.SelectedIndex = -1;
                cbTuesday.SelectedIndex = -1;
                cbWednesday.SelectedIndex = -1;
                cbThursday.SelectedIndex = -1;
                cbFriday.SelectedIndex = -1;

                var selectedCourse = ((DataGrid)sender).SelectedItem as Course;
                if(selectedCourse != null)
                {
                    foreach(WorkDay day in selectedCourse.Schedule.Keys)
                    {
                        cbSchedule.SelectedItems.Add(day);
                        switch(day)
                        {
                            case WorkDay.Monday:
                                cbMonday.SelectedValue = selectedCourse.Schedule[day].Item1;
                                break;
                            case WorkDay.Tuesday:
                                cbTuesday.SelectedValue = selectedCourse.Schedule[day].Item1;
                                break;
                            case WorkDay.Wednesday:
                                cbWednesday.SelectedValue = selectedCourse.Schedule[day].Item1;
                                break;
                            case WorkDay.Thursday:
                                cbThursday.SelectedValue = selectedCourse.Schedule[day].Item1;
                                break;
                            case WorkDay.Friday:
                                cbFriday.SelectedValue = selectedCourse.Schedule[day].Item1;
                                break;
                        }
                    }
                }
            }
        }
    }
}
