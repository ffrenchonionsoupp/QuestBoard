using SQLite;

namespace MAUI_QuestBoard.Models;

// Join entity between User and Event: one row = one person's RSVP to one event.
// This is what "events I'm attending" is queried from.
//
// There's no separate "UserId" foreign key column here - Email already
// captures who this is (prepopulated from the logged-in user's own Email
// when they RSVP, or manually typed by a guest), so it does double duty
// as both contact info AND the identity link back to User.Email when it
// happens to match a real account.
public class RSVP
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int EventId { get; set; }

    [Indexed]
    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
