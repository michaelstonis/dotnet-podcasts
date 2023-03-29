namespace Microsoft.NetConf2021.Maui.Pages;

public partial class CategoriesPage : ContentPage
{
    CategoriesViewModel vm => BindingContext as CategoriesViewModel;
    public CategoriesPage(CategoriesViewModel vm)
    {
        BindingContext = vm;    
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await vm.InitializeAsync();
    }
}
