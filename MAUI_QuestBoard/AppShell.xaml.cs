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
            Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));
            Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));
            Routing.RegisterRoute(nameof(MyAdventuresPage), typeof(MyAdventuresPage));
            Routing.RegisterRoute(nameof(MyQuestsPage), typeof(MyQuestsPage));
            Routing.RegisterRoute(nameof(RSVPPage), typeof(RSVPPage));
        }
    }
}
