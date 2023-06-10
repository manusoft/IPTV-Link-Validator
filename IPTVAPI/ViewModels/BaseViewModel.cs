using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using IPTVAPI.Core.Models;

namespace IPTVAPI.ViewModels;
public partial class BaseViewModel : ObservableRecipient
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOfflineNotBusy))]
    bool isOfflineBusy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOnlineNotBusy))]
    bool isOnlineBusy;

    [ObservableProperty]
    bool isValidationBusy;

    [ObservableProperty]
    string? title;

    [ObservableProperty]
    string? offlineStatus;

    [ObservableProperty]
    string? onlineStatus;

    public bool IsOfflineNotBusy => !IsOfflineBusy;
    public bool IsOnlineNotBusy => !IsOnlineBusy;

    [ObservableProperty]
    int onlineChannelsCount;

    [ObservableProperty]
    int offlineChannelsCount;

    [ObservableProperty]
    int onlineStreamsCount;

    [ObservableProperty]
    int offlineStreamsCount;

    [ObservableProperty]
    int onlineStreamsRemovedCount;

    [ObservableProperty]
    string? lastOnlineUpdateAt;

    [ObservableProperty]
    string? lastOfflineUpdateAt;

    [ObservableProperty]
    int newChannelsCount;

    [ObservableProperty]
    int newStreamsCount;

    [ObservableProperty]
    Int32 pivotSelected;

    public ObservableCollection<OnlineChannel> OnlineChannelList { get; } = new ObservableCollection<OnlineChannel>();
    public ObservableCollection<OnlineStream> OnlineStreamList { get; } = new ObservableCollection<OnlineStream>();
    public ObservableCollection<OfflineChannel> OfflineChannelList { get; } = new ObservableCollection<OfflineChannel>();
    public ObservableCollection<OfflineStream> OfflineStreamsList { get; } = new ObservableCollection<OfflineStream>();
}
