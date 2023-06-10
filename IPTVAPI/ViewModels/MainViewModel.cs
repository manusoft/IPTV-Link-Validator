using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IPTVAPI.Contracts.ViewModels;
using IPTVAPI.Core.Models;
using IPTVAPI.Core.Services;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace IPTVAPI.ViewModels;

public partial class MainViewModel : BaseViewModel, INavigationAware
{
    private readonly DataService dataService;
    private readonly FetchService fetchService;
    private CancellationTokenSource? cancellationTokenSource;
    public ObservableCollection<OfflineStream> CurrentStreamList { get; set; } = new ObservableCollection<OfflineStream>();

    public MainViewModel()
    {
        dataService = App.GetService<DataService>();
        fetchService = App.GetService<FetchService>();

        CommandText = "Start Validation";
    }

    public void OnNavigatedFrom() { }
    public async void OnNavigatedTo(object parameter)
    {
        PivotSelected = 0;
        await PopulateOfflineDataAsync();
    }

    [RelayCommand]
    private async void Refresh()
    {
        await PopulateOfflineDataAsync();
    }

    private async Task PopulateOfflineDataAsync()
    {
        if (IsOfflineBusy) return;

        try
        {
            IsOfflineBusy = true;

            OfflineChannelsCount = 0;
            OfflineStreamsCount = 0;

            OfflineChannelList.Clear();
            OfflineStreamsList.Clear();

            var offChannels = await dataService.GetChannelsAsync();

            foreach (var channel in offChannels)
            {
                OfflineChannelList.Add(channel);
            }

            var offStreams = await dataService.GetStreamsAsync();

            foreach (var stream in offStreams)
            {
                OfflineStreamsList.Add(stream);
            }

            if (OfflineChannelList is not null)
            {
                OfflineChannelsCount = offChannels.Count();

                foreach (var channel in offChannels)
                {
                    OfflineStreamsCount = OfflineStreamsCount + channel.Streams.Count;
                }

                NewChannelsCount = OnlineChannelsCount - OfflineChannelsCount;
                NewStreamsCount = OnlineStreamsCount - OfflineStreamsCount;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsOfflineBusy = false;
            LastOfflineUpdateAt = DateTime.Now.ToString("F");
        }
    }

    [RelayCommand]
    private async Task FetchData()
    {
        await PopulateOnlineDataAsync();
    }

    private async Task PopulateOnlineDataAsync()
    {
        if (IsOnlineBusy) return;

        try
        {
            IsOnlineBusy = true;

            OnlineChannelsCount = 0;
            OnlineStreamsCount = 0;
            OnlineStreamsRemovedCount = 0;

            OnlineChannelList.Clear();

            OnlineStatus = "Fetching channels ...";

            var channels = await fetchService.GetChannelsAsync();

            OnlineStatus = "Fetching streams ...";

            var streams = await fetchService.GetStreamsAsync();

            if (channels.Count == 0)
            {
                OnlineStatus = "Fetching faild.";
                return;
            }

            foreach (var channel in channels)
            {
                var availableStreams = streams.FindAll(s => s.Channel == channel.Id);

                if (availableStreams.Count is not 0)
                {
                    OnlineChannelList.Add(channel);
                }
                else
                {
                    foreach (var stream in availableStreams)
                    {
                        streams.Remove(stream);
                        OnlineStreamsRemovedCount++;
                    }
                }
            }

            foreach (var stream in streams)
            {
                if (!string.IsNullOrEmpty(stream.Channel))
                {
                    OnlineStreamList.Add(stream);
                }
                else
                {
                    OnlineStreamsRemovedCount++;
                }
            }

            if (OnlineChannelList is not null)
            {
                OnlineChannelsCount = OnlineChannelList.Count();
                NewChannelsCount = OnlineChannelsCount - OfflineChannelsCount;
            }

            if (OnlineStreamList is not null)
            {
                OnlineStreamsCount = OnlineStreamList.Count();
                NewStreamsCount = OnlineStreamsCount - OfflineStreamsCount;
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            IsOnlineBusy = false;
            OnlineStatus = "Done";
            LastOnlineUpdateAt = DateTime.Now.ToString("F");
        }
    }


    [RelayCommand]
    void CheckLinks()
    {
        if (OnlineChannelsCount == 0)
        {
            StatusText = "Online data is empty, please fetch from online first and try again.";
            return;
        }

        if (IsValidationBusy)
        {
            StopProcess();
            StatusText = "Process Completed";
        }
        else
        {
            StatusText = "Process Started";
            CommandText = "Stop Validation";
            StartProcess();
        }

    }

    private async void StartProcess()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            IsValidationBusy = true;
            // Disable the button or perform any other necessary UI updates

            // Perform your asynchronous operations here
            await ValidationAsync(cancellationToken);

            // Process completed successfully
            // ...
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation if needed
            // ...
        }
        finally
        {
            // Reset the flag and re-enable the button
            IsValidationBusy = false;
            // Enable the button or perform any other necessary UI updates
        }
    }

    private void StopProcess()
    {
        cancellationTokenSource?.Cancel();
    }

    async Task ValidationAsync(CancellationToken cancellationToken)
    {
        try
        {
            CompletedCount = 0;

            if (OnlineStreamList is null) return;

            TotalStreamsCount = OnlineStreamList.Count();

            // Check all online streams
            foreach (var stream in OnlineStreamList)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ProgressText = $"{CompletedCount} of {TotalStreamsCount}";
                ChannelName = stream.Channel;
                StatusText = stream.Url;
                UpdatedDate = DateTime.Now.ToString();

                // Get stream from database
                var getOffStream = OfflineStreamsList.FirstOrDefault(s => s.Url == stream.Url);

                // Stream found in the databsae
                if (getOffStream is not null)
                {
                    TimeSpan timeSpan = DateTime.Now - getOffStream.LastCheckedAt;

                    // Check if exceed 7 days
                    if (timeSpan.Days > 7)
                    {
                        var isValid = await IsUrlAccessible(getOffStream.Url, cancellationToken);

                        if (isValid) // valid then set online status as true
                        {
                            OfflineStream offlineStream = new OfflineStream()
                            {
                                Id = getOffStream.Id,
                                ChannelId = getOffStream.ChannelId,
                                Url = getOffStream.Url,
                                IsOnline = true,
                                LastCheckedAt = DateTime.Now,
                            };

                            await dataService.UpdateStreamAsync(offlineStream);

                            OnlineCount++;
                        }
                        else // not valid then online startus as false
                        {
                            OfflineStream offlineStream = new OfflineStream()
                            {
                                Id = getOffStream.Id,
                                ChannelId = getOffStream.ChannelId,
                                Url = getOffStream.Url,
                                IsOnline = false,
                                LastCheckedAt = DateTime.Now,
                            };

                            await dataService.UpdateStreamAsync(offlineStream);

                            OfflineCount++;
                        }
                    }
                }
                else // Stream not found in the database
                {
                    var isValid = await IsUrlAccessible(stream.Url, cancellationToken);

                    if (isValid) // valid then set online status as true
                    {
                        var channel = OnlineChannelList.FirstOrDefault(c => c.Id == stream.Channel);

                        if (channel is not null)
                        {
                            OfflineChannel offlineChannel = new OfflineChannel()
                            {
                                Id = channel.Id,
                                Name = channel.Name,
                                Country = channel.Country,
                                Languages = String.Join(',', channel.Languages),  // read value.Split(',').ToList()
                                Categories = String.Join(',', channel.Categories),
                                IsNsfw = channel.IsNsfw,
                                Website = channel.Website,
                                Logo = channel.Logo,
                            };

                            OfflineStream offlineStream = new OfflineStream()
                            {
                                ChannelId = stream.Channel,
                                Url = stream.Url,
                                IsOnline = true,
                                LastCheckedAt = DateTime.Now,
                            };

                            await dataService.CreateOrUpdateAsync(offlineChannel, offlineStream);

                            OnlineCount++;
                        }
                    }
                    else // not valid then online startus as false
                    {
                        var channel = OnlineChannelList.FirstOrDefault(c => c.Id == stream.Channel);

                        if (channel is not null)
                        {
                            OfflineChannel offlineChannel = new OfflineChannel()
                            {
                                Id = channel.Id,
                                Name = channel.Name,
                                Country = channel.Country,
                                Languages = String.Join(',', channel.Languages),  // read value.Split(',').ToList()
                                Categories = String.Join(',', channel.Categories),
                                IsNsfw = channel.IsNsfw,
                                Website = channel.Website,
                                Logo = channel.Logo,
                            };

                            OfflineStream offlineStream = new OfflineStream()
                            {
                                ChannelId = stream.Channel,
                                Url = stream.Url,
                                IsOnline = false,
                                LastCheckedAt = DateTime.Now,
                            };

                            await dataService.CreateOrUpdateAsync(offlineChannel, offlineStream);
                            OfflineCount++;
                        }
                    }
                }
                //await PopulateOfflineDataAsync();
                CompletedCount++;
                Debug.WriteLine(CompletedCount);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            CommandText = "Start Validation";
        }
    }
    public static async Task<bool> IsUrlAccessible(string url, CancellationToken cancellationToken)
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url), cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return false;  // URL is inaccessible
        }
    }

    [RelayCommand]
    async Task SaveToJsonAsync()
    {
        await PopulateOfflineDataAsync();

        if (OfflineChannelList is null) return;
        if (OfflineStreamsList is null) return;

        List<RootChannel> rootChannels = new List<RootChannel>();
        List<RootStream> rootStreams = new List<RootStream>();

        var onlineStreams = OfflineStreamsList.Where(stream => stream.IsOnline is true);

        foreach (var channel in OfflineChannelList)
        {
            List<OfflineStream> streams = onlineStreams.Where(stream => stream.ChannelId == channel.Id).ToList();

            if (streams.Count is not 0)
            {
                rootStreams.Clear();

                foreach (var stream in streams)
                {
                    rootStreams.Add(new RootStream
                    {
                        Url = stream.Url,
                    });
                }

                rootChannels.Add(new RootChannel
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Country = channel.Country,
                    Languages = channel.Languages,
                    Categories = channel.Categories,
                    IsNsfw = channel.IsNsfw,
                    Website = channel.Website,
                    Logo = channel.Logo,
                    Streams = rootStreams,
                });
            }
        }

        Root root = new Root();
        root.Total = rootChannels.Count;
        root.LastUpdated = DateTime.Now.ToString();
        root.Channels = rootChannels;

        // Serialize the object to a JSON string
        string jsonContent = JsonConvert.SerializeObject(root, Formatting.Indented);
        File.WriteAllText(@"c:\backup\root.json", jsonContent);
        Debug.WriteLine("File Saved.");
    }


    // Properties
    [ObservableProperty]
    int totalStreamsCount;

    [ObservableProperty]
    int completedCount;

    [ObservableProperty]
    int offlineCount;

    [ObservableProperty]
    int onlineCount;

    [ObservableProperty]
    string? progressText;

    [ObservableProperty]
    string? channelName;

    [ObservableProperty]
    string? statusText;

    [ObservableProperty]
    string? commandText;

    [ObservableProperty]
    string? updatedDate;
}
