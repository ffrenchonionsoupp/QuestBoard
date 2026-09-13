using MAUI_QuestBoard.Models;

namespace MAUI_QuestBoard.Services;

public static class DataService
{
    public static User ValidUser = new User
    {
        UserId = "fraham5822",
        Password = "Password1",
        Name = "Francis Hampton",
        Email = "fraham5822@students.ecpi.edu",
        Phone = "000-1234"
    };

    public static List<Event> Events = new()
    {
        new Event {
            Id = 1,
            Name = "Finish the Crawl",
            Host = "DungeonCrawlerCarl",
            Location = "Floor 4",
            Date = DateTime.Now.AddDays(3),
            Category = "Adventure",
            MaxAttendees = 6,
            CurrentAttendees = 3,
            Description = "A beginner-friendly dungeon crawl."
        },
        new Event {
            Id = 2,
            Name = "Wizard’s Council Meetup",
            Host = "GaleWaterDeep",
            Location = "Waterdeep",
            Date = DateTime.Now.AddDays(7),
            Category = "Magic",
            MaxAttendees = 12,
            CurrentAttendees = 8,
            Description = "Discuss spells, scrolls, and arcane lore."
        },
        new Event {
            Id = 3,
            Name = "Ranger’s Forest Trek",
            Host = "LeafWalker",
            Location = "Emerald Forest",
            Date = DateTime.Now.AddDays(10),
            Category = "Nature",
            MaxAttendees = 10,
            CurrentAttendees = 4,
            Description = "A scenic trek through the forest."
        }
    };

    public static List<int> UserAttending = new() { 1 };
    public static List<int> UserHosting = new() { 3 };


    public static User GuestUser { get; internal set; } = new User
    {
        UserId = "guest",
        Password = "guest",
        Name = "Guest User",
        Email = "guest@example.com",
        Phone = "000-0000"
    };
}
