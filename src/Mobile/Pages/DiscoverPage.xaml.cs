namespace Microsoft.NetConf2021.Maui.Pages;

public partial class DiscoverPage : ContentPage
{
    private DiscoverViewModel viewModel => BindingContext as DiscoverViewModel;

    public DiscoverPage(DiscoverViewModel vm)
    {
        BindingContext = vm;    
        InitializeComponent();
    }

    protected override async void OnHandlerChanging(HandlerChangingEventArgs args)
    {
        base.OnHandlerChanging(args);

        if (args.NewHandler is null)
        {
            return;
        }

        await viewModel.InitializeCommand.ExecuteAsync(null);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        player.OnAppearing();
    }


    protected override void OnDisappearing()
    {
        player.OnDisappearing();
        base.OnDisappearing();
    }
}
