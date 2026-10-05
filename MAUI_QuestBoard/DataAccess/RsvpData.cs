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
        DatabaseDiagnostics.RecordConnection(nameof(RsvpData));
        await database.CreateTableAsync<RSVP>();
    }

    public async Task<List<RSVP>> GetRsvpsForUserAsync(string email)
    {
        await Init();
        return await database.Table<RSVP>().Where(r => r.Email == email).ToListAsync();
    }

    public async Task<List<RSVP>> GetRsvpsForEventAsync(int eventId)
    {
        await Init();
        return await database.Table<RSVP>().Where(r => r.EventId == eventId).ToListAsync();
    }

    // Used to enforce "prevent an individual from submitting multiple
    // RSVP requests for the same event" - checked by Email since that's
    // the only identity guests have too.
    public async Task<bool> HasRsvpedAsync(int eventId, string email)
    {
        await Init();
        var existing = await database.Table<RSVP>()
            .Where(r => r.EventId == eventId && r.Email == email)
            .FirstOrDefaultAsync();
        return existing is not null;
    }

    public async Task<int> SaveRsvpAsync(RSVP rsvp)
    {
        await Init();
        return await database.InsertAsync(rsvp);
    }
}
