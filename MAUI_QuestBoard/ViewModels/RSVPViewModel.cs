using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class RSVPViewModel : BaseViewModel, IQueryAttributable
{
    private readonly RsvpData _rsvpData = new();
    private readonly EventData _eventData = new();

    public Event? SelectedEvent { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(Error);

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public RSVPViewModel()
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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Event", out var eventObj) && eventObj is Event @event)
        {
            SelectedEvent = @event;
        }
        else
        {
            SelectedEvent = new Event { Host = "Unknown Host", Name = "Unnamed Event", Address = "Unknown Address" };
        }
        OnPropertyChanged(nameof(SelectedEvent));

        // Logged-in users get their profile info prefilled; guests get blank
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

        SetError(string.Empty);
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(Phone));
    }

    private async void OnSave()
    {
        if (SelectedEvent is null) return;

        var problems = new List<string>();

        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(Name)) missing.Add("Name");
        if (string.IsNullOrWhiteSpace(Email)) missing.Add("Email");
        if (string.IsNullOrWhiteSpace(Phone)) missing.Add("Phone");
        if (missing.Count > 0)
        {
            problems.Add($"Please fill in: {string.Join(", ", missing)}.");
        }

        if (!string.IsNullOrWhiteSpace(Email) && !ValidationHelper.IsValidEmail(Email))
        {
            problems.Add("Enter a valid email address (for example, name@example.com).");
        }

        if (!string.IsNullOrWhiteSpace(Phone) && !ValidationHelper.IsValidPhone(Phone))
        {
            problems.Add("Phone number can only contain digits, spaces, dashes, dots, parentheses and +, with at least 7 digits.");
        }

        if (problems.Count > 0)
        {
            SetError(string.Join(Environment.NewLine, problems));
            return;
        }

        var email = Email.Trim();
        var eventName = SelectedEvent.Name;

        try
        {
            var freshEvent = await _eventData.GetEventAsync(SelectedEvent.Id) ?? SelectedEvent;
            eventName = freshEvent.Name;

            if (DateTime.Now > freshEvent.RsvpDeadline)
            {
                SetError("The RSVP deadline for this event has passed.");
                return;
            }

            if (freshEvent.CurrentAttendees >= freshEvent.MaxAttendees)
            {
                SetError("This event has reached its maximum number of attendees.");
                return;
            }

            if (await _rsvpData.HasRsvpedAsync(freshEvent.Id, email))
            {
                SetError("You have already RSVP'd for this event.");
                return;
            }

            await _rsvpData.SaveRsvpAsync(new RSVP
            {
                EventId = freshEvent.Id,
                Email = email,
                Name = Name.Trim(),
                Phone = Phone.Trim()
            });

            freshEvent.CurrentAttendees += 1;
            await _eventData.SaveEventAsync(freshEvent);
        }
        catch (Exception ex)
        {
            SetError($"Could not save your RSVP: {ex.Message}");
            return;
        }

        SetError(string.Empty);
        await Shell.Current.DisplayAlert("RSVP confirmed", $"You're signed up for {eventName}.", "OK");
        await NavigationHelper.GoBackAsync();
    }

    private async void OnCancel()
    {
        await NavigationHelper.GoBackAsync();
    }
}
