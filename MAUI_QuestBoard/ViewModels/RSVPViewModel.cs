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
            SelectedEvent = new Event { Host = "Unknown Host", Name = "Unnamed Event", Address = "Unknown Address" };
        }
        OnPropertyChanged(nameof(SelectedEvent));

        // Logged-in users get their profile info prefilled; guests get blank,
        // editable fields, per the RSVP page requirements.
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
        if (SelectedEvent is null) return;

        if (string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Phone))
        {
            Error = "All fields are required.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        try
        {
            // Re-fetch the event fresh from the database rather than trusting
            // the object we navigated in with, since CurrentAttendees may have
            // changed since this page was opened.
            var freshEvent = await _eventData.GetEventAsync(SelectedEvent.Id) ?? SelectedEvent;

            if (DateTime.Now > freshEvent.RsvpDeadline)
            {
                Error = "The RSVP deadline for this event has passed.";
                OnPropertyChanged(nameof(Error));
                return;
            }

            if (freshEvent.CurrentAttendees >= freshEvent.MaxAttendees)
            {
                Error = "This event has reached its maximum number of attendees.";
                OnPropertyChanged(nameof(Error));
                return;
            }

            if (await _rsvpData.HasRsvpedAsync(freshEvent.Id, Email))
            {
                Error = "You have already RSVP'd for this event.";
                OnPropertyChanged(nameof(Error));
                return;
            }

            await _rsvpData.SaveRsvpAsync(new RSVP
            {
                EventId = freshEvent.Id,
                Email = Email,
                Name = Name,
                Phone = Phone
            });

            freshEvent.CurrentAttendees += 1;
            await _eventData.SaveEventAsync(freshEvent);
        }
        catch (Exception ex)
        {
            Error = $"Could not save your RSVP: {ex.Message}";
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
