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
    }
}
