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
    public string Location { get; set; } = string.Empty;
    public string Max { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Deadline { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
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
            string.IsNullOrWhiteSpace(Location) ||
            string.IsNullOrWhiteSpace(Max) ||
            string.IsNullOrWhiteSpace(Date) ||
            string.IsNullOrWhiteSpace(Deadline) ||
            string.IsNullOrWhiteSpace(Category) ||
            string.IsNullOrWhiteSpace(Description))
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
            HostUserId = SessionService.IsLoggedIn ? SessionService.CurrentUser.UserId : null,
            Location = Location,
            Date = eventDate,
            RsvpDeadline = rsvpDeadline,
            Category = Category,
            MaxAttendees = maxAttendees,
            CurrentAttendees = 0,
            Description = Description
        };

        await _eventData.SaveEventAsync(newEvent);

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
