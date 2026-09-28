using SQLite;

namespace MAUI_QuestBoard.Models;

public class Event
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // Denormalized display name of the host, shown on cards/details without a join.
    public string Host { get; set; } = string.Empty;

    // Real link to the user who created the event (their Email), used to
    // answer "events I'm hosting" regardless of what display name was typed.
    public string? HostEmail { get; set; }

    public string Address { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime RsvpDeadline { get; set; }
    public int MaxAttendees { get; set; }
    public int CurrentAttendees { get; set; }
}
