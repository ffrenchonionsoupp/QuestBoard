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
                    Host = "DungeonCrawlerCarl",
                    HostUserId = "fraham5822",
                    Location = "Floor 4",
                    Date = DateTime.Now.AddDays(3),
                    RsvpDeadline = DateTime.Now.AddDays(2),
                    Category = "Adventure",
                    MaxAttendees = 6,
                    CurrentAttendees = 3,
                    Description = "A beginner-friendly dungeon crawl."
                },
                new Event
                {
                    Name = "Wizard's Council Meetup",
                    Host = "GaleWaterDeep",
                    HostUserId = null,
                    Location = "Waterdeep",
                    Date = DateTime.Now.AddDays(7),
                    RsvpDeadline = DateTime.Now.AddDays(6),
                    Category = "Magic",
                    MaxAttendees = 12,
                    CurrentAttendees = 8,
                    Description = "Discuss spells, scrolls, and arcane lore."
                },
                new Event
                {
                    Name = "Ranger's Forest Trek",
                    Host = "LeafWalker",
                    HostUserId = null,
                    Location = "Emerald Forest",
                    Date = DateTime.Now.AddDays(10),
                    RsvpDeadline = DateTime.Now.AddDays(9),
                    Category = "Nature",
                    MaxAttendees = 10,
                    CurrentAttendees = 4,
                    Description = "A scenic trek through the forest."
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

    public async Task<List<Event>> GetEventsHostedByAsync(string userId)
    {
        await Init();
        return await database.Table<Event>()
            .Where(e => e.HostUserId == userId)
            .OrderBy(e => e.Date)
            .ToListAsync();
    }

    public async Task<int> SaveEventAsync(Event ev)
    {
        await Init();
        if (ev.Id != 0)
        {
            // Update an existing event
            return await database.UpdateAsync(ev);
        }
        // Save a new event
        return await database.InsertAsync(ev);
    }
}
