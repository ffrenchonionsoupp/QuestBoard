using System.Windows.Input;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Pages;
using MAUI_QuestBoard.ViewModels;

namespace MAUI_QuestBoard.ViewModels;

public class EventDetailsViewModel : BaseViewModel, IQueryAttributable
{
    public Event SelectedEvent { get; set; } = new Event
    {
        Host = string.Empty,
        Name = string.Empty,
        Location = string.Empty,
        Category = string.Empty,
        Description = string.Empty
    };

    public ICommand RSVPCommand { get; }

    public EventDetailsViewModel()
    {
        RSVPCommand = new Command(OnRSVP);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Event", out var eventObj) && eventObj is Event eventValue)
        {
            SelectedEvent = eventValue;
            OnPropertyChanged(nameof(SelectedEvent));
        }
        else
        {
            SelectedEvent = new Event
            {
                Host = string.Empty,
                Name = string.Empty,
                Location = string.Empty,
                Category = string.Empty,
                Description = string.Empty
            }; // Fallback to avoid null
            OnPropertyChanged(nameof(SelectedEvent));
        }
    }

    private async void OnRSVP()
    {
        await Shell.Current.GoToAsync($"{nameof(RSVPPage)}",
            new Dictionary<string, object> { { "Event", SelectedEvent } });
    }
}
