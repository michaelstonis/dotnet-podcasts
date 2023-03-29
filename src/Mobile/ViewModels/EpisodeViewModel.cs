namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class EpisodeViewModel : ViewModelBase
{
    readonly PlayerService playerService;

    [ObservableProperty]
    Episode episode;

    public bool IsInListenLater
    {
        get
        {
            return Episode.IsInListenLater;
        }
        set
        {
            Episode.IsInListenLater = value;
            OnPropertyChanged();
        }
    }

    public ShowViewModel Show { get; set; }

    public EpisodeViewModel(
        Episode episode,
        ShowViewModel show,
        PlayerService player)
    {
        playerService = player;

        Episode = episode;
        Show = show;
    }

    [RelayCommand]
    Task PlayEpisode() => playerService.PlayAsync(Episode, Show.Show);

    [RelayCommand]
    Task NavigateToDetail() =>
        Shell.Current.GoToAsync(
            nameof(EpisodeDetailPage),
            new Dictionary<string, object>
            {
                [nameof(Show)] = Show,
                [nameof(Episode)] = Episode,
            });
}
