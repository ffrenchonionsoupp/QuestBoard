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
    public ICommand NavigateToAdminCommand { get; }
    public ICommand LogoutCommand { get; }

    // Drives whether the Admin button is shown - only the administrator sees it.
    public bool IsAdmin { get; private set; }

    public QuestBoardViewModel()
    {
        SelectEventCommand = new Command<Event>(OnSelectEvent);
        NavigateToMyAdventuresCommand = new Command(OnNavigateToMyAdventures);
        NavigateToMyQuestsCommand = new Command(OnNavigateToMyQuests);
        NavigateToAddEventCommand = new Command(OnNavigateToAddEvent);
        NavigateToAdminCommand = new Command(OnNavigateToAdmin);
        LogoutCommand = new Command(OnLogout);
    }

    public async Task LoadAsync()
    {
        IsAdmin = SessionService.IsAdmin;
        OnPropertyChanged(nameof(IsAdmin));

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

    // My Adventures and My Quests are tabs, so these switch tabs with an
    // absolute route ("//") rather than pushing a second copy of the page.
    private async void OnNavigateToMyAdventures()
    {
        await Shell.Current.GoToAsync("//MyAdventuresPage");
    }

    private async void OnNavigateToMyQuests()
    {
        await Shell.Current.GoToAsync("//MyQuestsPage");
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

    private async void OnNavigateToAdmin()
    {
        await Shell.Current.GoToAsync(nameof(AdminPage));
    }

    private async void OnLogout()
    {
        SessionService.SignOut();
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
