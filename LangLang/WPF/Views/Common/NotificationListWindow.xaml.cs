using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.Views.Factories;

namespace LangLang.WPF.Views.Common;

public partial class NotificationListWindow : NavigableWindow
{
    public NotificationListWindow(NotificationListViewModel notificationListViewModel, ILangLangWindowFactory windowFactory)
        : base(notificationListViewModel, windowFactory)
    {
        InitializeComponent();
        DataContext = notificationListViewModel;
    }
}