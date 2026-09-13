namespace MAUI_QuestBoard
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Initialize the main window and set the root page to AppShell
            return new Window(new AppShell());
        }
    }
}