using MAUI_QuestBoard.Models;
using SQLite;

namespace MAUI_QuestBoard.DataAccess;

public class EventData
{
    SQLiteAsyncConnection database;

    async Task Init()
    {
        if (database is not null)
        {
            return;
        }

        database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        DatabaseDiagnostics.RecordConnection(nameof(EventData));
        await database.CreateTableAsync<Event>();

        await SeedAsync();
    }

    private async Task SeedAsync()
    {
        var count = await database.Table<Event>().CountAsync();
        if (count == 0)
        {
            await database.InsertAllAsync(new[]
            {
                new Event
                {
                    Name = "Finish the Crawl",
                    Host = "Francis Hampton",
                    HostEmail = "fraham5822@students.ecpi.edu",
                    Address = "123 Dungeon Way, Floor 4",
                    Date = DateTime.Now.AddDays(3),
                    RsvpDeadline = DateTime.Now.AddDays(2),
                    MaxAttendees = 6,
                    CurrentAttendees = 0
                },
                new Event
                {
                    Name = "Wizard's Council Meetup",
                    Host = "Gale Waterdeep",
                    HostEmail = null,
                    Address = "1 Arcane Tower, Waterdeep",
                    Date = DateTime.Now.AddDays(7),
                    RsvpDeadline = DateTime.Now.AddDays(6),
                    MaxAttendees = 12,
                    CurrentAttendees = 0
                },
                new Event
                {
                    Name = "Ranger's Forest Trek",
                    Host = "Leaf Walker",
                    HostEmail = null,
                    Address = "Trailhead, Emerald Forest",
                    Date = DateTime.Now.AddDays(10),
                    RsvpDeadline = DateTime.Now.AddDays(9),
                    MaxAttendees = 10,
                    CurrentAttendees = 0
                }
            });
        }
    }

    public async Task<List<Event>> GetEventsAsync()
    {
        await Init();
        return await database.Table<Event>().OrderBy(e => e.Date).ToListAsync();
    }

    public async Task<Event?> GetEventAsync(int id)
    {
        await Init();
        return await database.Table<Event>().Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Event>> GetEventsHostedByAsync(string hostEmail)
    {
        await Init();
        return await database.Table<Event>()
            .Where(e => e.HostEmail == hostEmail)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<int> SaveEventAsync(Event ev)
    {
        await Init();
        if (ev.Id != 0)
        {
            // Update an existing event (used when incrementing CurrentAttendees on RSVP)
            return await database.UpdateAsync(ev);
        }
        // Save a new event
        return await database.InsertAsync(ev);
    }
}
