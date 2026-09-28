using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Models;
using MAUI_QuestBoard.Services;

namespace MAUI_QuestBoard.ViewModels;

public class AddUserViewModel : BaseViewModel
{
    private readonly UserData _userData = new();

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
        if (string.IsNullOrWhiteSpace(Name) ||
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

        var existing = await _userData.GetUserAsync(Email);
        if (existing is not null)
        {
            Error = "An account with that email already exists.";
            OnPropertyChanged(nameof(Error));
            return;
        }

        try
        {
            await _userData.SaveUserAsync(new User
            {
                Email = Email,
                Password = Password1,
                Name = Name,
                Phone = Phone
            });
        }
        catch (Exception ex)
        {
            Error = $"Could not create the account: {ex.Message}";
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
