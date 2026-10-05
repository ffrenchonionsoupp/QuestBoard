namespace MAUI_QuestBoard
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // The QuestBoard palette is a light, parchment-based design, so stay
            // light even when Windows is in dark mode (otherwise native controls
            // and default text switch to dark-mode colors on a parchment page).
            UserAppTheme = Microsoft.Maui.ApplicationModel.AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Initialize the main window and set the root page to AppShell
            return new Window(new AppShell());
        }
    }
}