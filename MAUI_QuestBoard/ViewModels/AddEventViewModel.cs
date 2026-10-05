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

    // The pickers can't produce a malformed date, and the event date picker
    // won't let anyone scroll back to a day that has already passed.
    public DateTime MinEventDate { get; } = DateTime.Today;

    public DateTime EventDate { get; set; } = DateTime.Today.AddDays(1);
    public TimeSpan EventTime { get; set; } = new TimeSpan(18, 0, 0);

    public DateTime DeadlineDate { get; set; } = DateTime.Today.AddDays(1);
    public TimeSpan DeadlineTime { get; set; } = new TimeSpan(12, 0, 0);

    public string Error { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(Error);

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddEventViewModel()
    {
        SaveCommand = new Command(OnSave);
        CancelCommand = new Command(OnCancel);
    }

    private void SetError(string message)
    {
        Error = message;
        OnPropertyChanged(nameof(Error));
        OnPropertyChanged(nameof(HasError));
    }

    private async void OnSave()
    {
        // Collect every problem so the person can fix them all in one pass.
        // Nothing the person typed is cleared
        var problems = new List<string>();

        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(Name)) missing.Add("Event Name");
        if (string.IsNullOrWhiteSpace(Host)) missing.Add("Host Name");
        if (string.IsNullOrWhiteSpace(Address)) missing.Add("Event Address");
        if (string.IsNullOrWhiteSpace(Max)) missing.Add("Max Attendees");
        if (missing.Count > 0)
        {
            problems.Add($"Please fill in: {string.Join(", ", missing)}.");
        }

        var maxAttendees = 0;
        if (!string.IsNullOrWhiteSpace(Max) &&
            (!int.TryParse(Max.Trim(), out maxAttendees) || maxAttendees < 1))
        {
            problems.Add("Max Attendees must be a whole number of 1 or more.");
        }

        var eventStart = EventDate.Date + EventTime;
        var rsvpDeadline = DeadlineDate.Date + DeadlineTime;

        if (eventStart <= DateTime.Now)
        {
            problems.Add("The event date and time must be in the future.");
        }

        if (rsvpDeadline > eventStart)
        {
            problems.Add("The RSVP deadline must be at or before the event date and time.");
        }

        if (problems.Count > 0)
        {
            SetError(string.Join(Environment.NewLine, problems));
            return;
        }

        var newEvent = new Event
        {
            Name = Name.Trim(),
            Host = Host.Trim(),
            // Ties the event to whoever is logged in, so it shows up under
            // "My Quests" regardless of what host name was typed above.
            // Guests aren't real accounts, so their events aren't attributed to anyone.
            HostEmail = SessionService.IsLoggedIn ? SessionService.CurrentUser.Email : null,
            Address = Address.Trim(),
            Date = eventStart,
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
            SetError($"Could not save the event: {ex.Message}");
            return;
        }

        SetError(string.Empty);
        await Shell.Current.DisplayAlert("Event saved", $"\"{newEvent.Name}\" was added to the Quest Board.", "OK");
        await NavigationHelper.GoBackAsync();
    }

    private async void OnCancel()
    {
        await NavigationHelper.GoBackAsync();
    }
}
