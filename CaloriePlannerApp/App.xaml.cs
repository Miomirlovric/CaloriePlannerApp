using CaloriePlannerApp.Data.ViewModels;

namespace CaloriePlannerApp
{
    public partial class App : Application
    {
        private ShellVM ShellVM;
        public App(ShellVM shellVM)
        {
            InitializeComponent();
            ShellVM = shellVM;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(ShellVM));
        }
    }
}