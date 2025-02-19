using CaloriePlannerApp.Data.ViewModels;

namespace CaloriePlannerApp;

public partial class UserSelection : ContentPage
{
	public UserSelection(UserSelectionVM userSelectionVM)
	{
		InitializeComponent();
        this.BindingContext = userSelectionVM;
    }

    protected override async void OnAppearing()
    {
        var x = (UserSelectionVM)BindingContext;
        await x.OnAppearing();
        base.OnAppearing();
    }
}