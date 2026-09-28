using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

// Guests never get persisted to the database - they're a local-only
// placeholder identity so RSVP/session code has something to point at.
public static class DataService
{
    private static List<User> users = new()
    {
        new User { Email = "admin@example.com", Password = "admin123", Name = "Admin User", Phone = "123-456-7890", Role = "Admin" },
        new User { Email = "guest@example.com", Password = "guest", Name = "Guest User", Phone = "000-000-0000", Role = "Guest" },
        new User { Email = "francis@example.com", Password = "password", Name = "Francis Hampton", Phone = "987-654-3210", Role = "User" }
    };

    public static List<User> GetAllUsers()
    {
        return users;
    }

    public static User? ValidateUser(string email, string password)
    {
        return users.FirstOrDefault(u =>
            u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.Password.Equals(password.Trim()));
    }

    // Expose the GuestUser as a property
    public static User GuestUser => users.First(u => u.Role == "Guest");
}
