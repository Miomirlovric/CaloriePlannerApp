using CaloriePlannerApp.Data.ViewModels;
using CaloriePlannerApp.Data;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CaloriePlannerApp.Data.Services;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Hosting;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core.Extensions;

namespace CaloriePlannerApp;

public class FoodItemModel
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}
public partial class FoodSelectPopupVM : ObservableObject
{
    private readonly DataContext dataContext;
    private Action close;
    private Action<Guid> selected;
    [ObservableProperty]
    public ObservableCollection<FoodItemModel> foods;
    [ObservableProperty]
    private string keywords;

    private const int Take = 50;

    private int TotalCount = 0;
    public FoodSelectPopupVM(DataContext dataContext, Action close, Action<Guid> selected)
    {
        this.dataContext = dataContext;
        this.close = close;
        this.selected = selected;
    }
    public async Task Init()
    {
        Foods = dataContext.Foods.Where(x => x.Name.Contains(Keywords)).Take(Take).Select(x => new FoodItemModel() { Id = x.Id, Name = x.Name }).ToObservableCollection();
        TotalCount = dataContext.Foods.Where(x => x.Name.Contains(Keywords)).Count();

        this.PropertyChanged += (a, e) =>
        {
            if(e.PropertyName == "Keywords")
            {
                Foods = dataContext.Foods.Where(x => x.Name.Contains(Keywords)).Take(Take).Select(x => new FoodItemModel() { Id = x.Id, Name = x.Name }).ToObservableCollection();
                TotalCount = dataContext.Foods.Where(x => x.Name.Contains(Keywords)).Count();
            }
        };
    }

    [RelayCommand]
    private void LoadMore()
    {
        if(Foods.Count < TotalCount)
        {
            var foodsDb = dataContext.Foods.Where(x => x.Name.Contains(Keywords)).Skip(Foods.Count).Take(Take).ToList();
            foreach(var i in foodsDb.Select(x => new FoodItemModel() { Id = x.Id, Name = x.Name }))
            {
                Foods.Add(i);
            }
        }
    }
    [RelayCommand]
    private void SelectFood(FoodItemModel item)
    {
        selected.Invoke(item.Id);
        close.Invoke();
    }

}

public partial class FoodSelectPopup : Popup
{
    private FoodSelectPopupVM vm;
    public FoodSelectPopup(DataContext dataContext, Action<Guid> saved)
    {
        vm = new FoodSelectPopupVM(dataContext, () => this.Close(), saved);
        BindingContext = vm;
        InitializeComponent();
    }
    protected override async void OnParentSet()
    {
        await vm.Init();
        base.OnParentSet();
    }
}