using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // LoginPage and QuestBoardPage are declared as <ShellContent> in
            // AppShell.xaml, which already gives them routes - registering
            // them again here creates ambiguous routes and breaks ".." pops.

            // Pages reached by pushing (GoToAsync) are registered here only.
            Routing.RegisterRoute(nameof(AddUserPage), typeof(AddUserPage));
            Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));
            Routing.RegisterRoute(nameof(EventDetailsPage), typeof(EventDetailsPage));
            Routing.RegisterRoute(nameof(MyAdventuresPage), typeof(MyAdventuresPage));
            Routing.RegisterRoute(nameof(MyQuestsPage), typeof(MyQuestsPage));
            Routing.RegisterRoute(nameof(RSVPPage), typeof(RSVPPage));
        }
    }
}
