using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly UserData _userData = new();
    private readonly AuthWebService _authWebService = new();

    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

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
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            Message = "Enter your email and password.";
            OnPropertyChanged(nameof(Message));
            return;
        }

        // Validate user credentials in the local database
        var user = await _userData.ValidateUserAsync(Email, Password);

        if (user is null)
        {
            Message = "Login failed.";
            OnPropertyChanged(nameof(Message));
            return;
        }

        // Verify credentials with the authentication service
        var verified = await _authWebService.AuthenticateAsync(Email, Password);

        if (!verified)
        {
            Message = "Could not verify credentials with the authentication service. Make sure it's running.";
            OnPropertyChanged(nameof(Message));
            return;
        }

        // Set the session for the logged-in user
        SessionService.IsLoggedIn = true;
        SessionService.CurrentUser = user;

        // Redirect based on user role
        if (SessionService.IsAdmin)
        {
            await Shell.Current.GoToAsync("//AdminPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//QuestBoardPage");
        }
    }

    private async void OnContinueAsGuest()
    {
        SessionService.IsLoggedIn = false;
        SessionService.CurrentUser = DataService.GuestUser;

        await Shell.Current.GoToAsync("//QuestBoardPage");
    }

    private async void OnCreateAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddUserPage));
    }
}
