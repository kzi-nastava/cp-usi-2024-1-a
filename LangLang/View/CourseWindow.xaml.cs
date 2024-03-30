using LangLang.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for CourseWindow.xaml
    /// </summary>
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
    }
}
