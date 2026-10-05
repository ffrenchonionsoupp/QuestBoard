using SQLite;

namespace MAUI_QuestBoard.Models;

public class User
{
    [PrimaryKey]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // "User" for normal registered accounts, "Admin" for the administrator.
    // (The in-memory guest placeholder uses "Guest" - guests are never saved.)
    public string Role { get; set; } = "User";
}
