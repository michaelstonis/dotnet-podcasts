namespace Microsoft.NetConf2021.Maui.ViewModels;

public partial class ShowViewModel : ObservableObject
{
    private readonly ImageProcessingService imageProcessingService;

    bool isInitialized;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Author))]
    [NotifyPropertyChangedFor(nameof(Title))]
    [NotifyPropertyChangedFor(nameof(Description))]
    [NotifyPropertyChangedFor(nameof(Episodes))]
    public Show show;

    [ObservableProperty]
    bool isSubscribed;

    [ObservableProperty]
    string cachedImage;

    public IEnumerable<Episode> Episodes => Show?.Episodes;

    public string Author => Show?.Author;

    public string Title => Show?.Title;

    public string Description => Show?.Description;

    public ShowViewModel(Show show, bool isSubscribed, ImageProcessingService imageProcessing)
    {
        Show = show;
        IsSubscribed = isSubscribed;
        imageProcessingService = imageProcessing;
    }

    [RelayCommand]
    private Task NavigateToDetail() =>
        Shell.Current.GoToAsync(
            nameof(ShowDetailPage),
            new Dictionary<string, object>
            {
                [nameof(ShowDetailViewModel.Show)] = this,
            });

    [RelayCommand]
    private async Task InitializeAsync()
    {
        if (isInitialized)
        {
            return;
        }

        try
        {
            CachedImage = await imageProcessingService.ProcessRemoteImage(Show.Image);
        }
        catch
        {
            return;
        }

        isInitialized = true;
    }
}
