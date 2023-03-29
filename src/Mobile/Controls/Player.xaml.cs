namespace Microsoft.NetConf2021.Maui.Controls;

public partial class Player : ContentView
{   
    private PlayerService playerService;
    private ImageProcessingService imageProcessingService;
    
    public Player()
    {
        InitializeComponent();
        AutomationProperties.SetIsInAccessibleTree(this, true);
        this.IsVisible = false;

#if WINDOWS || MACCATALYST
        this.HeightRequest = 90;
#elif ANDROID || IOS
        this.HeightRequest = 70;
#endif
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (playerService == null)
        {
            this.playerService = this.Handler.MauiContext.Services.GetService<PlayerService>();
            this.imageProcessingService = this.Handler.MauiContext.Services.GetService<ImageProcessingService>();
            await InitPlayerAsync();
        }
    }

    private async void PlayGesture_Tapped(object sender, EventArgs e)
    {
        await playerService.PlayAsync(playerService.CurrentEpisode, playerService.CurrentShow);
    }

    internal async void OnAppearing()
    {
        await InitPlayerAsync();  
    }

    async Task InitPlayerAsync()
    {
        if (playerService == null)
            return;

        this.playerService.IsPlayingChanged += PlayerService_IsPlayingChanged;
        IsVisible = playerService.CurrentEpisode != null;

        if (this.playerService.CurrentEpisode != null)
            await UpdatePlayPauseAsync();
    }

    private async Task UpdatePlayPauseAsync()
    {
        this.IsVisible = true;

        this.playButton.Source = this.playerService.IsPlaying ? "player_pause.png" : "player_play.png";

        episodeTitle.Text = this.playerService.CurrentEpisode.Title;
        authorText.Text = $"{this.playerService.CurrentShow?.Author} - {this.playerService.CurrentEpisode?.Published.ToString("MMM, d yyy")}";

        podcastImage.Source =
            imageProcessingService is not null && this.playerService.CurrentShow?.Image is not null
                ? await imageProcessingService?.ProcessRemoteImage(this.playerService.CurrentShow?.Image)
                : this.playerService.CurrentShow?.Image;
        duration.Text = this.playerService.CurrentEpisode?.Duration.ToString();
    }

    private async void PlayerService_IsPlayingChanged(object sender, EventArgs e)
    {
        if (this.playerService.CurrentEpisode == null)
        {
            IsVisible = false;
        }
        else
        {
            await UpdatePlayPauseAsync();
        }
    }

    internal void OnDisappearing()
    {
        this.playerService.IsPlayingChanged -= PlayerService_IsPlayingChanged;
    }
}

