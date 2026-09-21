using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

// Guests never get persisted to the database - they're a local-only
// placeholder identity so RSVP/session code has something to point at.
public static class DataService
{
    public static User GuestUser { get; internal set; } = new User
    {
        UserId = "guest",
        Password = "guest",
        Name = "Guest User",
        Email = "guest@example.com",
        Phone = "000-0000"
    };
}
