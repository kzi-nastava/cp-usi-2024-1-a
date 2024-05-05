using LangLang.MVVM;
using LangLang.View.Factories;
using LangLang.ViewModel;

namespace LangLang.View;

public partial class NotificationWindow : NavigableWindow
{
    public NotificationWindow(NotificationViewModel notificationViewModel, ILangLangWindowFactory windowFactory)
        : base(notificationViewModel, windowFactory)
    {
        InitializeComponent();
        DataContext = notificationViewModel;
    }
}