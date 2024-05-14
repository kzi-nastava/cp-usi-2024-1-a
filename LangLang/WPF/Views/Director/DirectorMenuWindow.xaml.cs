using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Director;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Director
{
    /// <summary>
    /// Interaction logic for DirectorWindow.xaml
    /// </summary>
    public partial class DirectorMenuWindow : NavigableWindow
    {
        public DirectorMenuWindow(DirectorMenuViewModel directorMenuViewModel, ILangLangWindowFactory windowFactory) 
            : base(directorMenuViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = directorMenuViewModel;
        }
    }
}
