using MvvmHelpers;

namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class ListenLaterViewModel : ViewModelBase
{
    readonly ListenLaterService listenLaterService;
    readonly PlayerService playerService;
    private readonly SubscriptionsService subscriptionsService;
    private readonly ImageProcessingService imageProcessingService;

    public ObservableRangeCollection<EpisodeViewModel> Episodes { get; } = new ObservableRangeCollection<EpisodeViewModel>();

    public ListenLaterViewModel(ListenLaterService listen, PlayerService player, SubscriptionsService subs, ImageProcessingService imageProcessing)
    {
        listenLaterService = listen;
        playerService = player;
        subscriptionsService = subs;
        imageProcessingService = imageProcessing;
    }

    internal Task InitializeAsync()
    {
        var episodes = listenLaterService.GetEpisodes();

        var list = new List<EpisodeViewModel>();
        foreach (var episode in episodes)
        {
            var episodeVM = new EpisodeViewModel(episode.Episode, new ShowViewModel(episode.Show, subscriptionsService.IsSubscribed(episode.Show.Id), imageProcessingService), playerService);

            list.Add(episodeVM);
        }
        Episodes.ReplaceRange(list);

        return Task.CompletedTask;
    }

    [RelayCommand]
    void Remove(EpisodeViewModel episode)
    {
        var episodeToRemove = Episodes
            .FirstOrDefault(ep => ep.Episode.Id == episode.Episode.Id);

        if(episodeToRemove != null)
        {
            listenLaterService.Remove(episode.Episode);
            Episodes.Remove(episodeToRemove);
            episode.IsInListenLater = false;
        }
    }
}

