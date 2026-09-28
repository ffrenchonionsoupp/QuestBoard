using SQLite;

namespace MAUI_QuestBoard.Models;

public class User
{
    [PrimaryKey]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // New property to define the user's role
    public string Role { get; set; } = "Guest"; // Default role is Guest
}
