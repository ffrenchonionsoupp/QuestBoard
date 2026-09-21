using SQLite;

namespace MAUI_QuestBoard.DataAccess;

public static class DatabaseConstants
{
    public const string DatabaseFilename = "QuestBoard.db3";

    public const SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite
        // create the database if it doesn't exist
        | SQLiteOpenFlags.Create
        // enable multi-threading (needed since UserData, EventData, and
        // RsvpData each open their own connection to the same file)
        | SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}
