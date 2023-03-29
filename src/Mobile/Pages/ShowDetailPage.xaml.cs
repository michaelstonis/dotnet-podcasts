namespace Microsoft.NetConf2021.Maui.Pages;

public partial class ShowDetailPage : ContentPage
{
    private ShowDetailViewModel viewModel => BindingContext as ShowDetailViewModel;

    public ShowDetailPage(ShowDetailViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }

    protected override async void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        base.OnHandlerChanging(args);

        if(args.NewHandler is null)
        {
            return;
        }

        await viewModel.InitializeCommand.ExecuteAsync(null);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.player.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        this.player.OnDisappearing();
        base.OnDisappearing();
    }
}
