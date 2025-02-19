using CaloriePlannerApp.Data.Entities;
using CaloriePlannerApp.Data.Services;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.Controls.Internals.Profile;

namespace CaloriePlannerApp.Data.ViewModels
{
    public partial class FoodPerDayItem : ObservableObject
    {
        public Guid? id;
        public Guid foodId;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string foodGroup;

        [ObservableProperty]
        private int calories;

        [ObservableProperty]
        private decimal fat;

        [ObservableProperty]
        private decimal protein;

        [ObservableProperty]
        private decimal carbohydrate;

        [ObservableProperty]
        private decimal sugars;

        [ObservableProperty]
        private decimal fiber;

        [ObservableProperty]
        private int quantity;
    }
    public partial class FoodInputsVM : ObservableObject
    {
        private readonly DataContext dataContext;
        private readonly UserService userService;

        [ObservableProperty]
        private DateTime date;
        [ObservableProperty]
        private ObservableCollection<FoodPerDayItem> foods = new();

        [ObservableProperty]
        private int caloriesSum;

        [ObservableProperty]
        private decimal fatSum;

        [ObservableProperty]
        private decimal proteinSum;

        [ObservableProperty]
        private decimal carbohydrateSum;

        [ObservableProperty]
        private decimal sugarsSum;

        [ObservableProperty]
        private decimal fiberSum;

        public FoodInputsVM(DataContext dataContext, UserService userService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
        }

        public async Task OnAppearing()
        {
            this.Date = DateTime.Now;
            SetFoodForDay(Date);

            PropertyChanged += (e,a) =>
            {
                if(a.PropertyName == "Date")
                {
                    SetFoodForDay(Date);
                }
            };
        }

        public void SetFoodForDay(DateTime cdate)
        {
            var dbfoods = dataContext.FoodUsers.Include(x => x.Food).Where(x => x.At.Date == cdate.Date && x.UserId == userService.CurrentUser.Id).ToList();
            Foods = dbfoods.Select(x => new FoodPerDayItem()
            {
                Calories = x.Food.Calories,
                Carbohydrate = x.Food.Carbohydrate,
                Fat = x.Food.Fat,
                Fiber = x.Food.Fiber,
                FoodGroup = x.Food.FoodGroup,
                Name = x.Food.Name,
                id = x.Id,
                Protein = x.Food.Protein,
                Sugars = x.Food.Sugars,
                Quantity = x.Quantity,
                foodId = x.FoodId
            }).ToObservableCollection();

            foreach (var food in foods) 
            { 

                food.PropertyChanged += (a, x) => {
                    if(x.PropertyName == "Quantity")
                    {
                        CalculateTotalMacros();
                    }
                };
            }

            CalculateTotalMacros();
        }

        public void CalculateTotalMacros()
        {
            CaloriesSum = Foods.Sum(item => (int)Math.Round(item.Quantity * (item.Calories / 100.0)));
            FatSum = Foods.Sum(item => item.Quantity * item.Fat / 100);
            ProteinSum = Foods.Sum(item => item.Quantity * item.Protein / 100);
            CarbohydrateSum = Foods.Sum(item => item.Quantity * item.Carbohydrate / 100);
            SugarsSum = Foods.Sum(item => item.Quantity * item.Sugars / 100);
            FiberSum = Foods.Sum(item => item.Quantity * item.Fiber / 100);
        }

        [RelayCommand]
        public void NextDay()
        {
            Date = Date.AddDays(1);
            SetFoodForDay(Date);
        }
        [RelayCommand]
        public void New()
        {
            Shell.Current.ShowPopupAsync(new FoodSelectPopup(dataContext, SelectionFinished));
        }

        [RelayCommand]
        public void PreviosDay()
        {
            Date = Date.AddDays(-1);
            SetFoodForDay(Date);
        }
        [RelayCommand]
        public async Task Delete(FoodPerDayItem item)
        {
            if (item.id.HasValue)
            {
                var dbitem = dataContext.FoodUsers.FirstOrDefault(x => x.Id == item.id);
                if(dbitem != null)
                {
                    dataContext.FoodUsers.Remove(dbitem);
                    dataContext.SaveChanges();
                    Foods.Remove(item);
                    CalculateTotalMacros();
                }
            }
        }

        [RelayCommand]
        public void Save()
        {
            foreach(var food in Foods)
            {
                var dbFood = (FoodUser)null;

                if (food.id.HasValue)
                {
                    dbFood = dataContext.FoodUsers.FirstOrDefault(x => x.Id == food.id);
                }

                if(dbFood != null)
                {
                    dbFood.Quantity = food.Quantity;
                }
                else
                {
                    dataContext.FoodUsers.Add(new Entities.FoodUser()
                    {
                        At = Date,
                        FoodId= food.foodId,
                        Id = Guid.NewGuid(),
                        Quantity = food.Quantity,
                        UserId = userService.CurrentUser.Id
                    });
                }
                dataContext.SaveChanges();
            }
        }

        public void SelectionFinished(Guid FoodId)
        {
            var food = dataContext.Foods.FirstOrDefault(x => x.Id == FoodId);
            if(food != null)
            {
                if(Foods.Any(x => x.foodId == food.Id))
                {

                }
                else
                {
                    var fid = Guid.NewGuid();
                    dataContext.FoodUsers.Add(new Entities.FoodUser()
                    {
                        At = Date,
                        FoodId = food.Id,
                        Id = fid,
                        Quantity = 0,
                        UserId = userService.CurrentUser.Id
                    });

                    dataContext.SaveChanges();


                    var item = new FoodPerDayItem()
                    {
                        Calories = food.Calories,
                        Carbohydrate = food.Carbohydrate,
                        Fat = food.Fat,
                        Fiber = food.Fiber,
                        FoodGroup = food.FoodGroup,
                        Name = food.Name,
                        id = fid,
                        Protein = food.Protein,
                        Sugars = food.Sugars,
                        Quantity = 0,
                        foodId = food.Id
                    };

                    item.PropertyChanged += (a, x) => {
                        if (x.PropertyName == "Quantity")
                        {
                            CalculateTotalMacros();
                        }
                    };
                    Foods.Add(item);
                    CalculateTotalMacros();
                }

            }
        }
    }
}
