using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.ViewModels;

public class AddUserViewModel : BaseViewModel
{
    private readonly UserData _userData = new();

    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Password1 { get; set; } = string.Empty;
    public string Password2 { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    public ICommand AddCommand { get; }
    public ICommand CancelCommand { get; }

    public AddUserViewModel()
    {
        AddCommand = new Command(OnAdd);
        CancelCommand = new Command(OnCancel);
    }

    private async void OnAdd()
    {
        if (string.IsNullOrWhiteSpace(UserId) ||
            string.IsNullOrWhiteSpace(Name) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Phone) ||
            string.IsNullOrWhiteSpace(Password1) ||
            string.IsNullOrWhiteSpace(Password2))
        {
            Error = "All fields are required.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        if (Password1 != Password2)
        {
            Error = "Passwords do not match.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        var existing = await _userData.GetUserAsync(UserId);
        if (existing is not null)
        {
            Error = "That username is already taken.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        await _userData.SaveUserAsync(new User
        {
            UserId = UserId,
            Password = Password1,
            Name = Name,
            Email = Email,
            Phone = Phone
        });

        await Shell.Current.GoToAsync("..");
    }

    private async void OnCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}
