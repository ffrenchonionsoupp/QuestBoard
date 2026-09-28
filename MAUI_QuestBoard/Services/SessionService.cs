using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

public static class SessionService
{
    public static bool IsLoggedIn { get; set; }

    public static User CurrentUser { get; set; } = new User
    {
        Email = string.Empty,
        Password = string.Empty,
        Name = string.Empty,
        Phone = string.Empty,
        Role = "Guest" // Default role is Guest
    };

    // Helper property to check if the current user is an admin
    public static bool IsAdmin => CurrentUser.Role == "Admin";
}
