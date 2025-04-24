namespace MAUI_finalProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}