using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LangLang.Application.Stores;
using LangLang.Application.Utility.Notification;
using LangLang.Domain.Model;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.ViewModels.Common;

public class NotificationListViewModel : ViewModelBase, INavigableDataContext
{
    private readonly INotificationService _notificationService;
    private readonly INotificationInfoService _notificationInfoService;
    
    private readonly Profile currentProfile;
    
    public NavigationStore NavigationStore { get; }
    
    private ObservableCollection<NotificationViewModel> _notifications;

    private bool unreadOnly = true;
    public ICommand ToggleUnreadCommand { get; }
    public ICommand MarkAsReadCommand { get; }

    public NotificationListViewModel(IAuthenticationStore authenticationStore, INotificationService notificationService, INotificationInfoService notificationInfoService, NavigationStore navigationStore)
    {
        currentProfile = authenticationStore.CurrentUserProfile ??
                         throw new InvalidOperationException("Cannot view notifications without a logged in user.");
        _notificationService = notificationService;
        _notificationInfoService = notificationInfoService;
        NavigationStore = navigationStore;

        _notifications = new ObservableCollection<NotificationViewModel>();
        SetNotifications(notificationService.GetUnreadNotifications(currentProfile));
        
        ToggleUnreadCommand = new RelayCommand(_ => ToggleUnread());
        MarkAsReadCommand = new RelayCommand(MarkAsRead);
    }
    
    public ObservableCollection<NotificationViewModel> Notifications
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
        SetNotifications(UnreadOnly
            ? _notificationService.GetUnreadNotifications(currentProfile)
            : _notificationService.GetNotifications(currentProfile));
    }

    private void SetNotifications(List<Notification> notifications)
    {
        var senderNames = _notificationInfoService.GetSenderNames(notifications);
        Notifications = new ObservableCollection<NotificationViewModel>(notifications
            .Select(notification => new NotificationViewModel(senderNames[notification.Id], notification)));
    }


    private void MarkAsRead(object? parameter)
    {
        if (parameter is not NotificationViewModel notification) return;
        try
        {
            _notificationService.MarkAsRead(notification.Notification);
            if (UnreadOnly)
                Notifications.Remove(notification);
            else
                OnPropertyChanged(nameof(Notifications));
        }
        catch (ArgumentException e)
        {
            MessageBox.Show(e.Message);
        }
    }
}