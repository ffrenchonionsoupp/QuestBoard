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
        await database.CreateTableAsync<User>();

        await SeedAsync();
    }

    private async Task SeedAsync()
    {
        var count = await database.Table<User>().CountAsync();
        if (count == 0)
        {
            // Guaranteed login for grading/testing, same account used in Week 2.
            await database.InsertAsync(new User
            {
                UserId = "fraham5822",
                Password = "Password1",
                Name = "Francis Hampton",
                Email = "fraham5822@students.ecpi.edu",
                Phone = "000-1234"
            });
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        await Init();
        return await database.Table<User>().ToListAsync();
    }

    public async Task<User?> GetUserAsync(string userId)
    {
        await Init();
        return await database.Table<User>().Where(u => u.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<User?> ValidateUserAsync(string userId, string password)
    {
        await Init();
        return await database.Table<User>()
            .Where(u => u.UserId == userId && u.Password == password)
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
