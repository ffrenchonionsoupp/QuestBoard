using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

public static class SessionService
{
    public static bool IsLoggedIn { get; set; }

    // Removed 'required' modifier as it is not valid for static properties.
    // Initialized the property to avoid CS8618 error.
    public static User CurrentUser { get; set; } = new User
    {
        UserId = string.Empty,
        Password = string.Empty,
        Name = string.Empty,
        Email = string.Empty,
        Phone = string.Empty
    };
}
