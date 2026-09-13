namespace MAUI_QuestBoard.Models;

public class User
{
    // Fixed the syntax for required properties
    public required string UserId { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
}
