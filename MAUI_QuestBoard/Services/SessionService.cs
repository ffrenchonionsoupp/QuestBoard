using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

public static class SessionService
{
    public static bool IsLoggedIn { get; private set; }

    public static User CurrentUser { get; private set; } = DataService.GuestUser;

    public static bool IsAdmin => IsLoggedIn && CurrentUser.Role == "Admin";

    public static void SignIn(User user)
    {
        CurrentUser = user;
        IsLoggedIn = true;
    }

    // Used both for "Continue as Guest" and for logging out.
    public static void SignOut()
    {
        CurrentUser = DataService.GuestUser;
        IsLoggedIn = false;
    }
}
