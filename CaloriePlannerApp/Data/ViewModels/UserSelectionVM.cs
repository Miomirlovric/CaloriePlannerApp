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

namespace CaloriePlannerApp.Data.ViewModels
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public partial class UserSelectionVM : ObservableObject
    {
        private readonly DataContext dataContext;
        private readonly UserService userService;
        public UserSelectionVM(DataContext dataContext, UserService userService)
        {
            this.dataContext = dataContext;
            this.userService = userService;
        }
        [ObservableProperty]
        private ObservableCollection<UserModel> users;
        public async Task OnAppearing()
        {
            Users = dataContext.Users.Select(x => new UserModel { Id = x.Id, Name = x.UserFullName()}).ToObservableCollection();
        }
        [RelayCommand]
        private void SelectUser(UserModel user)
        {
            var userd = dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
            if(userd != null)
            {
                userService.CurrentUser = userd;
            }
        }
        [RelayCommand]
        private void DeleteUser(UserModel user)
        {
            var userd = dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
            if(userd != null)
            {
                dataContext.Users.Remove(userd);               
                dataContext.SaveChanges();
                if (userd.Id == userService.CurrentUser?.Id)
                {
                    userService.CurrentUser = null;
                }
                if (Users.Any(x => user.Id == x.Id))
                {
                    Users.Remove(user);
                }
            }
        }
        [RelayCommand]
        private void EditUser(UserModel user)
        {
            Shell.Current.ShowPopupAsync(new UserEditPopup(dataContext, user.Id, UserSaved));
        }

        [RelayCommand]
        private void New()
        {
            Shell.Current.ShowPopupAsync(new UserEditPopup(dataContext, null, UserSaved));
        }

        private void UserSaved(UserModel model)
        {
            var found = Users.FirstOrDefault(x => x.Id == model.Id);
            if(found != null)
            {
                Users = Users.Where(x => x.Id != model.Id).ToObservableCollection();
            }

            Users.Add(model);

        }
    }
}
