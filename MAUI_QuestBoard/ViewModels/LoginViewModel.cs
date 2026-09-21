using System.Windows.Input;
using MAUI_QuestBoard.DataAccess;
using MAUI_QuestBoard.Services;
using MAUI_QuestBoard.Pages;

namespace MAUI_QuestBoard.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly UserData _userData = new();

    public string UserId { get; set; } = string.Empty;
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
        if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Password))
        {
            Message = "Enter a username and password.";
            OnPropertyChanged(nameof(Message));
            return;
        }

        var user = await _userData.ValidateUserAsync(UserId, Password);

        if (user is not null)
        {
            SessionService.IsLoggedIn = true;
            SessionService.CurrentUser = user;

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
        SessionService.CurrentUser = DataService.GuestUser;

        await Shell.Current.GoToAsync("//QuestBoardPage");
    }

    private async void OnCreateAccount()
    {
        await Shell.Current.GoToAsync(nameof(AddUserPage));
    }
}
