using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input; 
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.ViewModels;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class LoginViewModel : BaseViewModel
{
    // Properties that will be bound to XAML
    public string UserId { get; set; } = string.Empty; // Initialize to avoid nullability issues
    public string Password { get; set; } = string.Empty; // Initialize to avoid nullability issues
    public string Message { get; set; } = string.Empty; // Initialize to avoid nullability issues

    // Commands that will be bound to buttons
    public ICommand LoginCommand { get; }
    public ICommand ContinueAsGuestCommand { get; }
    public ICommand CreateAccountCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new Command(OnLogin);
        ContinueAsGuestCommand = new Command(OnContinueAsGuest);
        CreateAccountCommand = new Command(OnCreateAccount);
    }

    private async void OnLogin()
    {
        if (UserId == DataService.ValidUser.UserId &&
            Password == DataService.ValidUser.Password)
        {
            SessionService.IsLoggedIn = true;
            SessionService.CurrentUser = DataService.ValidUser;

            await Shell.Current.GoToAsync("//QuestBoardPage");
        }
        else
        {
            Message = "Login failed.";
            OnPropertyChanged(nameof(Message));
        }
    }

    private async void OnContinueAsGuest()
    {
        SessionService.IsLoggedIn = false;
        SessionService.CurrentUser = DataService.GuestUser; // Use a predefined GuestUser instead of null

        await Shell.Current.GoToAsync("//QuestBoardPage");
    }

    private async void OnCreateAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddUserPage));
    }
}
