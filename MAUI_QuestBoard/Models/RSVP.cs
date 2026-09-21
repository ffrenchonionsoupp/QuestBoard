using SQLite;

namespace MAUI_QuestBoard.Models;

// Join entity between User and Event: one row = one person's RSVP to one event.
// This is what "events I'm attending" is queried from.
public class RSVP
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int EventId { get; set; }

    // Nullable because a guest (no account) can still RSVP.
    [Indexed]
    public string? UserId { get; set; }

    // Contact info captured at RSVP time (prepopulated from the user's
    // profile when logged in, blank and editable when a guest).
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
