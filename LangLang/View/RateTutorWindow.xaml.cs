using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View
{
    public partial class RateTutorWindow : NavigableWindow
    {
        public RateTutorWindow(RateTutorViewModel rateTutorViewModel, ILangLangWindowFactory windowFactory)
            : base(rateTutorViewModel, windowFactory)
        {
            InitializeComponent();
            DataContext = rateTutorViewModel;
        }

    }
}
