using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class AddEventViewModel : BaseViewModel
{
    private readonly EventData _eventData = new();

    public string Name { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Max { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Deadline { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEventViewModel()
    {
        SaveCommand = new Command(OnSave);
        CancelCommand = new Command(OnCancel);
    }

    private async void OnSave()
    {
        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Host) ||
            string.IsNullOrWhiteSpace(Address) ||
            string.IsNullOrWhiteSpace(Max) ||
            string.IsNullOrWhiteSpace(Date) ||
            string.IsNullOrWhiteSpace(Deadline))
        {
            Error = "All fields are required.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        if (!int.TryParse(Max, out var maxAttendees))
        {
            Error = "Max Attendees must be a number.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        if (!DateTime.TryParse(Date, out var eventDate))
        {
            Error = "Date is not a valid date/time.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        if (!DateTime.TryParse(Deadline, out var rsvpDeadline))
        {
            Error = "RSVP Deadline is not a valid date/time.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        var newEvent = new Event
        {
            Name = Name,
            Host = Host,
            // Ties the event to whoever is logged in, so it shows up under
            // "My Quests" regardless of what host name was typed above.
            // Guests aren't real accounts, so their events aren't attributed to anyone.
            HostEmail = SessionService.IsLoggedIn ? SessionService.CurrentUser.Email : null,
            Address = Address,
            Date = eventDate,
            RsvpDeadline = rsvpDeadline,
            MaxAttendees = maxAttendees,
            CurrentAttendees = 0
        };

        try
        {
            await _eventData.SaveEventAsync(newEvent);
        }
        catch (Exception ex)
        {
            Error = $"Could not save the event: {ex.Message}";
            OnPropertyChanged(nameof(Error));
            return;
        }

        await NavigationHelper.GoBackAsync();
    }

    private async void OnCancel()
    {
        await NavigationHelper.GoBackAsync();
    }
}
