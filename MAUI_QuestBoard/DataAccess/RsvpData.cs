using MAUI_QuestBoard.Models;
using SQLite;

namespace MAUI_QuestBoard.DataAccess;

public class RsvpData
{
    SQLiteAsyncConnection database;

    async Task Init()
    {
        if (database is not null)
        {
            return;
        }

        database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        await database.CreateTableAsync<RSVP>();
    }

    public async Task<List<RSVP>> GetRsvpsForUserAsync(string userId)
    {
        await Init();
        return await database.Table<RSVP>().Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<List<RSVP>> GetRsvpsForEventAsync(int eventId)
    {
        await Init();
        return await database.Table<RSVP>().Where(r => r.EventId == eventId).ToListAsync();
    }

    public async Task<int> SaveRsvpAsync(RSVP rsvp)
    {
        await Init();
        return await database.InsertAsync(rsvp);
    }
}
