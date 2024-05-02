using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using LangLang.Model;
using LangLang.MVVM;
using LangLang.Services.UtilityServices;
using LangLang.Stores;

namespace LangLang.ViewModel;

public class NotificationViewModel : ViewModelBase, INavigableDataContext
{
    private readonly INotificationService _notificationService;
    
    private readonly Profile currentProfile;
    
    public NavigationStore NavigationStore { get; }
    
    private ObservableCollection<Notification> _notifications;

    private bool unreadOnly = true;
    public ICommand ToggleUnreadCommand { get; }
    public ICommand MarkAsReadCommand { get; }

    public NotificationViewModel(IAuthenticationStore authenticationStore, INotificationService notificationService, NavigationStore navigationStore)
    {
        currentProfile = authenticationStore.CurrentUserProfile ??
                         throw new InvalidOperationException("Cannot view notifications without a logged in user.");
        _notificationService = notificationService;
        NavigationStore = navigationStore;
        
        _notifications = new ObservableCollection<Notification>(notificationService.GetUnreadNotifications(currentProfile));
        ToggleUnreadCommand = new RelayCommand(_ => ToggleUnread());
        MarkAsReadCommand = new RelayCommand(MarkAsRead);
    }
    
    public ObservableCollection<Notification> Notifications
    {
        get => _notifications;
        set => SetField(ref _notifications, value);
    }
    
    private bool UnreadOnly
    {
        get => unreadOnly;
        set
        {
            SetField(ref unreadOnly, value);
            OnPropertyChanged(nameof(ToggleButtonText));
        }
    }

    public string ToggleButtonText => UnreadOnly ? "Show all notifications" : "Show unread notifications only";
    
    private void ToggleUnread()
    {
        UnreadOnly = !UnreadOnly;
        Notifications = UnreadOnly
            ? new ObservableCollection<Notification>(_notificationService.GetUnreadNotifications(currentProfile))
            : new ObservableCollection<Notification>(_notificationService.GetNotifications(currentProfile));
    }

    private void MarkAsRead(object? parameter)
    {
        if (parameter is not Notification notification) return;
        try
        {
            _notificationService.MarkAsRead(notification);
            if (UnreadOnly)
                Notifications.Remove(notification);
        }
        catch (ArgumentException e)
        {
            MessageBox.Show(e.Message);
        }
    }
}