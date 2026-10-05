using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

// Guests never get persisted to the database - they're a local-only
// placeholder identity so RSVP/session code has something to point at.
// Real accounts (including the admin) live in the SQLite database and are
// read through UserData.
public static class DataService
{
    public static User GuestUser { get; } = new User
    {
        Email = "guest@example.com",
        Password = string.Empty,
        Name = "Guest User",
        Phone = string.Empty,
        Role = "Guest"
    };
}
