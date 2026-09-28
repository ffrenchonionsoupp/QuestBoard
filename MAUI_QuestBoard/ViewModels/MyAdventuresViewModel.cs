using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class MyAdventuresViewModel : BaseViewModel
{
    private readonly RsvpData _rsvpData = new();
    private readonly EventData _eventData = new();

    public ObservableCollection<Event> Events { get; } = new();

    // True while browsing as a guest - drives the "create an account" message.
    public bool IsGuest { get; private set; }

    public ICommand SelectEventCommand { get; }
    public ICommand CreateAccountCommand { get; }

    public MyAdventuresViewModel()
    {
        SelectEventCommand = new Command<Event>(OnSelectEvent);
        CreateAccountCommand = new Command(OnCreateAccount);
    }

    public async Task LoadAsync()
    {
        Events.Clear();

        IsGuest = !SessionService.IsLoggedIn;
        OnPropertyChanged(nameof(IsGuest));

        // Guests haven't RSVP'd under a real account, and nothing of theirs
        // is saved against an identity - there's nothing to show them here.
        if (IsGuest) return;

        var rsvps = await _rsvpData.GetRsvpsForUserAsync(SessionService.CurrentUser.Email);
        var eventIds = rsvps.Select(r => r.EventId).Distinct().ToHashSet();

        var allEvents = await _eventData.GetEventsAsync();
        foreach (var e in allEvents.Where(e => eventIds.Contains(e.Id)))
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
