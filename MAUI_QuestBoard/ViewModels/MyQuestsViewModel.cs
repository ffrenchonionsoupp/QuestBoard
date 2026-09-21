using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class MyQuestsViewModel : BaseViewModel
{
    private readonly EventData _eventData = new();

    public ObservableCollection<Event> Events { get; } = new();

    // True while browsing as a guest - drives the "create an account" message.
    public bool IsGuest { get; private set; }

    public ICommand SelectEventCommand { get; }
    public ICommand CreateAccountCommand { get; }

    public MyQuestsViewModel()
    {
        SelectEventCommand = new Command<Event>(OnSelectEvent);
        CreateAccountCommand = new Command(OnCreateAccount);
    }

    public async Task LoadAsync()
    {
        Events.Clear();

        IsGuest = !SessionService.IsLoggedIn;
        OnPropertyChanged(nameof(IsGuest));

        // Guests can't host anything that persists against an identity.
        if (IsGuest) return;

        var events = await _eventData.GetEventsHostedByAsync(SessionService.CurrentUser.UserId);
        foreach (var e in events)
            Events.Add(e);
    }

    private async void OnSelectEvent(Event evt)
    {
        await Shell.Current.GoToAsync($"{nameof(EventDetailsPage)}",
            new Dictionary<string, object> { { "Event", evt } });
    }

    private async void OnCreateAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddUserPage));
    }
}
