using Microsoft.NetConf2021.Maui.Resources.Strings;
using MvvmHelpers;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class ShowDetailViewModel : ViewModelBase, IQueryAttributable
{
    private readonly PlayerService playerService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ListenLaterService listenLaterService;
    private readonly ShowsService showsService;
    private readonly ImageProcessingService imageProcessingService;

    public ObservableRangeCollection<Episode> Episodes { get; } = new ObservableRangeCollection<Episode>();

    [ObservableProperty]
    ShowViewModel show;

    [ObservableProperty]
    Episode episodeForPlaying;

    [ObservableProperty]
    bool isPlaying;

    [ObservableProperty]
    string textToSearch;

    public ShowDetailViewModel(ShowsService shows, PlayerService player, SubscriptionsService subs, ListenLaterService later, ImageProcessingService imageProcessing)
    {
        showsService = shows;
        playerService = player;
        subscriptionsService = subs;
        listenLaterService = later;
        imageProcessingService = imageProcessing;
    }

    [RelayCommand]
    async Task InitializeAsync()
    {
        await FetchAsync();
    }

    async Task FetchAsync()
    {
        var updatedShow = await showsService.GetShowByIdAsync(Show.Show.Id);
        Show.Show = updatedShow;
        await Show.InitializeCommand.ExecuteAsync(null);
        Episodes.ReplaceRange(Show.Episodes);
    }

    [RelayCommand]
    void SearchEpisode()
    {
        var episodesList = Show.Episodes
            .Where(ep => ep.Title.Contains(TextToSearch, StringComparison.InvariantCultureIgnoreCase));

        Episodes.ReplaceRange(episodesList);
    }

    [RelayCommand]
    Task TapEpisode(Episode episode) =>
        Shell.Current.GoToAsync(
            nameof(EpisodeDetailPage),
            new Dictionary<string, object>
            {
                [nameof(Show)] = Show,
                [nameof(Episode)] = episode,
            });

    [RelayCommand]
    async Task Subscribe()
    {
        if (Show is null || subscriptionsService is null)
            return;

        if (Show.IsSubscribed)
        {
            var isUnsubcribe = await subscriptionsService.UnSubscribeFromShowAsync(Show.Show);
            Show.IsSubscribed = !isUnsubcribe;
        }
        else
        {
            subscriptionsService.SubscribeToShow(Show.Show);
            Show.IsSubscribed = true;
        }
    }

    [RelayCommand]
    Task PlayEpisode(Episode episode) => playerService.PlayAsync(episode, Show.Show);

    [RelayCommand]
    Task AddToListenLater(Episode episode)
    {
        var itemHasInListenLaterList = listenLaterService.IsInListenLater(episode);
        if (itemHasInListenLaterList)
        {
            listenLaterService.Remove(episode);
        }
        else
        {
            listenLaterService.Add(episode, Show.Show);
        }

        episode.IsInListenLater = !itemHasInListenLaterList;

        return Task.CompletedTask;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Show = query[nameof(Show)] as ShowViewModel;
    }
}
