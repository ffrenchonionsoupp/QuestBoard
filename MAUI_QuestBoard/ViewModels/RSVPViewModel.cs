using System.Windows.Input;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class RSVPViewModel : BaseViewModel, IQueryAttributable
{
    public Event SelectedEvent { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Error { get; set; }

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

        if (SessionService.IsLoggedIn)
        {
            Name = SessionService.CurrentUser.Name;
            Email = SessionService.CurrentUser.Email;
            Phone = SessionService.CurrentUser.Phone;
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

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
