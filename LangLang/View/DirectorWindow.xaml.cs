using LangLang.ViewModel;
using LangLang.MVVM;
using LangLang.View.Factories;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : NavigableWindow
    {
        public DirectorWindow(DirectorViewModel directorViewModel, ILangLangWindowFactory windowFactory) 
            : base(directorViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = directorViewModel;
        }
    }
}
