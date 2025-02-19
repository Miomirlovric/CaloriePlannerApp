using CaloriePlannerApp.Data.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CaloriePlannerApp
{
    public partial class AppShell : Shell
    {
        public AppShell(ShellVM shellVM)
        {
            InitializeComponent();
            this.BindingContext = shellVM;
            shellVM.OnAppearing();
        }
    }
}
