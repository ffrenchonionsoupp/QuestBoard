using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AddUserPage), typeof(AddUserPage));
            Routing.RegisterRoute(nameof(QuestBoardPage), typeof(QuestBoardPage));
        }
    }
}
