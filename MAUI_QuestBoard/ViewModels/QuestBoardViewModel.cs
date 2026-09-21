using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class QuestBoardViewModel : BaseViewModel
{
    private readonly EventData _eventData = new();

    public ObservableCollection<Event> Events { get; } = new();

    public ICommand SelectEventCommand { get; }
    public ICommand NavigateToMyAdventuresCommand { get; }
    public ICommand NavigateToMyQuestsCommand { get; }
    public ICommand NavigateToAddEventCommand { get; }
    public ICommand LogoutCommand { get; }

    public QuestBoardViewModel()
    {
        SelectEventCommand = new Command<Event>(OnSelectEvent);
        NavigateToMyAdventuresCommand = new Command(OnNavigateToMyAdventures);
        NavigateToMyQuestsCommand = new Command(OnNavigateToMyQuests);
        NavigateToAddEventCommand = new Command(OnNavigateToAddEvent);
        LogoutCommand = new Command(OnLogout);
    }

    public async Task LoadAsync()
    {
        var events = await _eventData.GetEventsAsync();

        Events.Clear();
        foreach (var e in events)
            Events.Add(e);
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
        if (!SessionService.IsLoggedIn)
        {
            await Shell.Current.DisplayAlert(
                "Account Required",
                "You need an account to host an event. Let's get you set up.",
                "OK");
            await Shell.Current.GoToAsync(nameof(AddUserPage));
            return;
        }

        await Shell.Current.GoToAsync(nameof(AddEventPage));
    }

    private async void OnLogout()
    {
        SessionService.IsLoggedIn = false;
        SessionService.CurrentUser = DataService.GuestUser;
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
