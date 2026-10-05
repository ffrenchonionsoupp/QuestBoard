using System.Collections.ObjectModel;
using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class EventDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly EventData _eventData = new();
    private readonly RsvpData _rsvpData = new();

    public Event SelectedEvent { get; set; } = new Event { Host = string.Empty, Name = string.Empty, Address = string.Empty };

    // Names of everyone who has RSVP'd
    public ObservableCollection<string> AttendeeNames { get; } = new();

    public ICommand RSVPCommand { get; }

    public EventDetailsViewModel()
    {
        RSVPCommand = new Command(OnRSVP);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Shell can call this again with an empty query when navigating back
        if (query.TryGetValue("Event", out var eventObj) && eventObj is Event eventValue)
        {
            SelectedEvent = eventValue;
            OnPropertyChanged(nameof(SelectedEvent));
        }
    }

    public async Task LoadAsync()
    {
        var fresh = await _eventData.GetEventAsync(SelectedEvent.Id);
        if (fresh is not null)
        {
            SelectedEvent = fresh;
            OnPropertyChanged(nameof(SelectedEvent));
        }

        var rsvps = await _rsvpData.GetRsvpsForEventAsync(SelectedEvent.Id);

        AttendeeNames.Clear();
        foreach (var r in rsvps)
            AttendeeNames.Add(r.Name);
    }

    private async void OnRSVP()
    {
        await Shell.Current.GoToAsync($"{nameof(RSVPPage)}",
            new Dictionary<string, object> { { "Event", SelectedEvent } });
    }
}
