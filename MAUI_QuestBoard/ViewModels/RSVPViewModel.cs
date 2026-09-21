using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class RSVPViewModel : BaseViewModel, IQueryAttributable
{
    private readonly RsvpData _rsvpData = new();

    public Event? SelectedEvent { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public RSVPViewModel()
    {
        SaveCommand = new Command(OnSave);
        CancelCommand = new Command(OnCancel);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Event", out var eventObj) && eventObj is Event @event)
        {
            SelectedEvent = @event;
        }
        else
        {
            SelectedEvent = new Event
            {
                Host = "Unknown Host",
                Name = "Unnamed Event",
                Location = "Unknown Location",
                Category = "General",
                Description = "No description available."
            };
        }
        OnPropertyChanged(nameof(SelectedEvent));

        if (SessionService.IsLoggedIn)
        {
            Name = SessionService.CurrentUser.Name;
            Email = SessionService.CurrentUser.Email;
            Phone = SessionService.CurrentUser.Phone;
        }
        else
        {
            Name = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }

        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(Phone));
    }

    private async void OnSave()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Phone))
        {
            Error = "All fields are required.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        if (SelectedEvent is not null)
        {
            await _rsvpData.SaveRsvpAsync(new RSVP
            {
                EventId = SelectedEvent.Id,
                UserId = SessionService.IsLoggedIn ? SessionService.CurrentUser.UserId : null,
                Name = Name,
                Email = Email,
                Phone = Phone
            });
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
