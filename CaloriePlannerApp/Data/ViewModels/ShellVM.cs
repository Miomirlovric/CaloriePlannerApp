using CaloriePlannerApp.Data.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriePlannerApp.Data.ViewModels
{
    public partial class ShellVM : ObservableObject
    {
        [ObservableProperty]
        public string userName;
        [ObservableProperty]
        public bool canSeeAll;
        private readonly UserService userService;
        public ShellVM(UserService userService)
        {
            this.userService = userService;
            UserName = userService.Name;
            CanSeeAll = UserName != "Select a user";
        }

        public void OnAppearing()
        {
            this.userService.userNameChanged = (userName) => { 
                this.UserName = userName;
                CanSeeAll = UserName != "Select a user";
            };
        }
    }
}
