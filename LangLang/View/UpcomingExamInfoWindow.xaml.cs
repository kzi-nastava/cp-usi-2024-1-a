using LangLang.DAO.JsonDao;
using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for ExamInfoViewModel.xaml
    /// </summary>
    public partial class UpcomingExamInfoWindow : NavigableWindow
    {
        private Brush _addedBrush = Brushes.Lime;
        private Brush _removedBrush = Brushes.Red;
        public UpcomingExamInfoWindow(UpcomingExamInfoViewModel upcomingExamInfoViewModel, ILangLangWindowFactory windowFactory)
            : base(upcomingExamInfoViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = upcomingExamInfoViewModel;
            upcomingExamInfoViewModel.Window = this;
        }

        public void AddClick()
        {
            UpdateRow();
        }
        public void RemoveClick()
        {
            UpdateRow();
        }


        private void UpdateRow()
        {
            UpcomingExamInfoViewModel viewModel = (DataContext as UpcomingExamInfoViewModel)!;
            DataGrid dataGrid = (FindName("studentDataGrid") as DataGrid)!;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(viewModel.SelectedStudent);
            if (viewModel.AddedStudents[viewModel.SelectedStudent!.Id]! == true)
                row.Background = _addedBrush;
            else
                row.Background = _removedBrush;
        }

        private void studentDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
            => e.Row.Background = _addedBrush;

        private void studentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count != 0)
            {
                UpcomingExamInfoViewModel viewModel = (DataContext as UpcomingExamInfoViewModel)!;
                DataGrid dataGrid = (FindName("studentDataGrid") as DataGrid)!;
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(e.RemovedItems[0]);
                if (viewModel.AddedStudents[viewModel.SelectedStudent!.Id]! == true)
                    row.Background = _addedBrush;
                else
                    row.Background = _removedBrush;
            }
            if (e.AddedItems.Count != 0)
            {
                UpcomingExamInfoViewModel viewModel = (DataContext as UpcomingExamInfoViewModel)!;
                DataGrid dataGrid = (FindName("studentDataGrid") as DataGrid)!;
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]);
                if (viewModel.AddedStudents[viewModel.SelectedStudent!.Id]! == true)
                    row.Background = _addedBrush;
                else
                    row.Background = _removedBrush;
            }
        }
    }
}
