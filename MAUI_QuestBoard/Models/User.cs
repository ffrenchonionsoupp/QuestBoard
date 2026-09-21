using SQLite;

namespace MAUI_QuestBoard.Models;

public class User
{
    // UserId is the chosen username and doubles as the primary key,
    // since it's already guaranteed unique by the "Add User" flow.
    [PrimaryKey]
    public string UserId { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
