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

    private void ShowMessage(string message)
    {
        Message = message;
        OnPropertyChanged(nameof(Message));
    }

    // The login page stays alive in the background after you sign in, so
    // wipe what was typed - otherwise the next person to log out lands on a
    // login screen with the previous email and password still filled in.
    private void ClearForm()
    {
        Email = string.Empty;
        Password = string.Empty;
        Message = string.Empty;
        OnPropertyChanged(nameof(Email));
        OnPropertyChanged(nameof(Password));
        OnPropertyChanged(nameof(Message));
    }

    private async void OnLogin()
    {
        var email = (Email ?? string.Empty).Trim();
        var password = Password ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
        {
            ShowMessage("Enter your email and password.");
            return;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            ShowMessage("Enter your email address.");
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ShowMessage("Enter your password.");
            return;
        }

        // Validate user credentials in the local database
        var user = await _userData.ValidateUserAsync(email, password);

        if (user is null)
        {
            // Deliberately doesn't say whether the email or the password was wrong.
            ShowMessage("Login failed.");
            return;
        }

        // Verify credentials with the authentication service (Basic Authentication)
        var verified = await _authWebService.AuthenticateAsync(email, password);

        if (!verified)
        {
            ShowMessage("Could not verify credentials with the authentication service. Make sure it's running.");
            return;
        }

        SessionService.SignIn(user);
        ClearForm();

        // Upon login, show the events the user is signed up to attend
        // (the administrator goes to the admin page instead).
        if (SessionService.IsAdmin)
        {
            // Admin is a pushed page, so go to the board first and push it on top.
            await Shell.Current.GoToAsync("//QuestBoardPage");
            await Shell.Current.GoToAsync(nameof(AdminPage));
        }
        else
        {
            await Shell.Current.GoToAsync("//MyAdventuresPage");
        }
    }

    private async void OnContinueAsGuest()
    {
        SessionService.SignOut();
        ClearForm();

        await Shell.Current.GoToAsync("//QuestBoardPage");
    }

    private async void OnCreateAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddUserPage));
    }
}
