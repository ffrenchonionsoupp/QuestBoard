namespace MAUI_QuestBoard.Models;

public class Event
{
    public int Id { get; set; }
    public required string Host { get; set; }
    public required string Name { get; set; } // Added 'required' modifier to fix CS8618
    public required string Location { get; set; }
    public DateTime Date { get; set; }
    public required string Category { get; set; }
    public int MaxAttendees { get; set; }
    public int CurrentAttendees { get; set; }
    public required string Description { get; set; }
}
