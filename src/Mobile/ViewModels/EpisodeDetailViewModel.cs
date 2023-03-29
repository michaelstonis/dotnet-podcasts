using Microsoft.NetConf2021.Maui.Models;
using Microsoft.NetConf2021.Maui.Resources.Strings;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class EpisodeDetailViewModel : ViewModelBase, IQueryAttributable
{
    private readonly ListenLaterService listenLaterService;
    private readonly ShowsService podcastService;
    private readonly PlayerService playerService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ImageProcessingService imageProcessingService;

    public bool IsInListenLater
    {
        get => Episode?.IsInListenLater ?? false;
        set
        {
            if (Episode is null)
                return;

            Episode.IsInListenLater = value;
            OnPropertyChanged();
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsInListenLater))]
    Episode episode;

    [ObservableProperty]
    private ShowViewModel show;

    public EpisodeDetailViewModel(ListenLaterService listen, ShowsService shows, PlayerService player, SubscriptionsService subs, ImageProcessingService imageProcessing)
    {
        listenLaterService = listen;
        podcastService = shows;
        playerService = player;
        subscriptionsService = subs;
        imageProcessingService = imageProcessing;
    }

    internal async Task InitializeAsync()
    {
        await FetchAsync();
    }

    async Task FetchAsync()
    {
        await Show.InitializeCommand.ExecuteAsync(null);

        IsInListenLater = listenLaterService.IsInListenLater(Episode);
    }

    [RelayCommand]
    void ListenLater()
    {
        if (Episode is null)
        {
            return;
        }

        if (listenLaterService.IsInListenLater(Episode))
            listenLaterService.Remove(Episode);
        else
            listenLaterService.Add(Episode, Show.Show);

        IsInListenLater = listenLaterService.IsInListenLater(Episode);

        var matchedEpisode = Show.Episodes.FirstOrDefault(x => x.Id == Episode.Id);

        if (matchedEpisode is not null)
        {
            matchedEpisode.IsInListenLater = IsInListenLater;
        }

        return;
    }

    [RelayCommand]
    Task Play() => playerService.PlayAsync(Episode, Show.Show);

    [RelayCommand]
    Task Share() => 
        Microsoft.Maui.ApplicationModel.DataTransfer.Share.RequestAsync(new ShareTextRequest
    {
        Text = $"{Config.BaseWeb}show/{Show.Show.Id}",
        Title = "Share the episode uri"
    });

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Show = query[nameof(Show)] as ShowViewModel;
        Episode = query[nameof(Episode)] as Episode;
    }
}
