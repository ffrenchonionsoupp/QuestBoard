using System.Windows.Input;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class QuestBoardViewModel : BaseViewModel
{
    public List<Event> Events { get; set; }

    public ICommand SelectEventCommand { get; }
    public ICommand NavigateToMyAdventuresCommand { get; }
    public ICommand NavigateToMyQuestsCommand { get; }
    public ICommand NavigateToAddEventCommand { get; }
    public ICommand LogoutCommand { get; }

    public QuestBoardViewModel()
    {
        Events = DataService.Events;
        SelectEventCommand = new Command<Event>(OnSelectEvent);
        NavigateToMyAdventuresCommand = new Command(OnNavigateToMyAdventures);
        NavigateToMyQuestsCommand = new Command(OnNavigateToMyQuests);
        NavigateToAddEventCommand = new Command(OnNavigateToAddEvent);
        LogoutCommand = new Command(OnLogout);
    }

    private async void OnSelectEvent(Event evt)
    {
        await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}",
            new Dictionary<string, object> { { "Event", evt } });
    }

    private async void OnNavigateToMyAdventures()
    {
        await Shell.Current.GoToAsync(nameof(MyAdventuresPage));
    }

    private async void OnNavigateToMyQuests()
    {
        await Shell.Current.GoToAsync(nameof(MyQuestsPage));
    }

    private async void OnNavigateToAddEvent()
    {
        await Shell.Current.GoToAsync(nameof(AddEventPage));
    }

    private async void OnLogout()
    {
        SessionService.IsLoggedIn = false;
        SessionService.CurrentUser = DataService.GuestUser;
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
