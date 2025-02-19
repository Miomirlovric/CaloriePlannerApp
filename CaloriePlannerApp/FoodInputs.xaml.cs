using CaloriePlannerApp.Data.ViewModels;

namespace CaloriePlannerApp;

public partial class FoodInputs : ContentPage
{
	public FoodInputs(FoodInputsVM userSelectionVM)
	{
        this.BindingContext = userSelectionVM;
        InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        var x = (FoodInputsVM)BindingContext;
        await x.OnAppearing();
        base.OnAppearing();
    }
}