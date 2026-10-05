using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Only pages that are reached by pushing (GoToAsync) are registered
            // here. LoginPage, QuestBoardPage, MyAdventuresPage and MyQuestsPage
            // are declared as <ShellContent> in AppShell.xaml
            Routing.RegisterRoute(nameof(AddUserPage), typeof(AddUserPage));
            Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));
            Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));
            Routing.RegisterRoute(nameof(RSVPPage), typeof(RSVPPage));
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
        }
    }
}
