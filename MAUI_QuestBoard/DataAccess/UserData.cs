using MAUI_QuestBoard.Models;
using SQLite;

namespace MAUI_QuestBoard.DataAccess;

public class UserData
{
    SQLiteAsyncConnection database;

    async Task Init()
    {
        if (database is not null)
        {
            return;
        }

        database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        DatabaseDiagnostics.RecordConnection(nameof(UserData));
        await database.CreateTableAsync<User>();

        await SeedAsync();
    }

    // Email of the administrator account.
    public const string AdminEmail = "admin@example.com";

    private async Task SeedAsync()
    {
        // Guaranteed logins for grading/testing.
        await EnsureUserAsync(new User
        {
            Email = "fraham5822@students.ecpi.edu",
            Password = "Password1",
            Name = "Francis Hampton",
            Phone = "000-1234",
            Role = "User"
        });

        await EnsureUserAsync(new User
        {
            Email = AdminEmail,
            Password = "admin123",
            Name = "Admin User",
            Phone = "123-4567",
            Role = "Admin"
        });
    }

    private async Task EnsureUserAsync(User user)
    {
        var existing = await database.Table<User>()
            .Where(u => u.Email == user.Email)
            .FirstOrDefaultAsync();

        if (existing is null)
        {
            await database.InsertAsync(user);
        }
        else if (existing.Role != user.Role)
        {
            // An account saved before roles existed has no role - bring the
            // seeded accounts up to date so the admin is actually an admin.
            existing.Role = user.Role;
            await database.UpdateAsync(existing);
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        await Init();
        return await database.Table<User>().ToListAsync();
    }

    public async Task<User?> GetUserAsync(string email)
    {
        await Init();
        return await database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        await Init();
        return await database.Table<User>()
            .Where(u => u.Email == email && u.Password == password)
            .FirstOrDefaultAsync();
    }

    public async Task<int> SaveUserAsync(User user)
    {
        await Init();
        // Accounts are only ever created new in this app - no edit-profile
        // flow yet - so this always inserts rather than branching on an ID.
        return await database.InsertAsync(user);
    }
}
