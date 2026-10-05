using SQLite;

namespace MAUI_QuestBoard.DataAccess;

// One row of "SELECT name FROM sqlite_master" - sqlite-net needs a public
// class with a parameterless constructor to read query results into.
public class TableNameRow
{
    public string Name { get; set; } = string.Empty;
}

public class DatabaseInfo
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FolderPath { get; set; } = string.Empty;
    public string FileExists { get; set; } = string.Empty;
    public string FileSize { get; set; } = string.Empty;
    public string LastModified { get; set; } = string.Empty;
    public string OpenFlags { get; set; } = string.Empty;
    public List<string> Tables { get; } = new();
    public List<string> Connections { get; } = new();
    public string? Error { get; set; }
}

// Admin-page helper: reports where the database file is, what is in it, and
// which connections the app has opened to it. Each data class (UserData,
// EventData, RsvpData) calls RecordConnection when it opens its connection.
public static class DatabaseDiagnostics
{
    private static readonly object Gate = new();
    private static readonly List<(string Owner, DateTime OpenedAt)> Opened = new();

    public static void RecordConnection(string owner)
    {
        lock (Gate)
        {
            Opened.Add((owner, DateTime.Now));
        }
    }

    public static async Task<DatabaseInfo> GetInfoAsync()
    {
        var info = new DatabaseInfo
        {
            FileName = DatabaseConstants.DatabaseFilename,
            FilePath = DatabaseConstants.DatabasePath,
            FolderPath = Path.GetDirectoryName(DatabaseConstants.DatabasePath) ?? string.Empty,
            OpenFlags = DatabaseConstants.Flags.ToString()
        };

        try
        {
            var file = new FileInfo(info.FilePath);
            info.FileExists = file.Exists ? "Yes" : "No";
            info.FileSize = file.Exists ? $"{file.Length:N0} bytes ({file.Length / 1024.0:N1} KB)" : "-";
            info.LastModified = file.Exists ? file.LastWriteTime.ToString("g") : "-";

            if (file.Exists)
            {
                // A separate short-lived connection just for reading the
                // table list and row counts. It is closed right away.
                var db = new SQLiteAsyncConnection(info.FilePath, DatabaseConstants.Flags);
                try
                {
                    var tables = await db.QueryAsync<TableNameRow>(
                        "SELECT name AS Name FROM sqlite_master " +
                        "WHERE type = 'table' AND name NOT LIKE 'sqlite_%' ORDER BY name");

                    foreach (var table in tables)
                    {
                        var rows = await db.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM \"{table.Name}\"");
                        info.Tables.Add($"{table.Name} - {rows} row(s)");
                    }
                }
                finally
                {
                    await db.CloseAsync();
                }
            }

            List<(string Owner, DateTime OpenedAt)> snapshot;
            lock (Gate)
            {
                snapshot = new List<(string Owner, DateTime OpenedAt)>(Opened);
            }

            if (snapshot.Count == 0)
            {
                info.Connections.Add("No connections have been opened yet this session.");
            }
            else
            {
                foreach (var group in snapshot.GroupBy(c => c.Owner).OrderBy(g => g.Key))
                {
                    info.Connections.Add(
                        $"{group.Key}: {group.Count()} connection(s) opened, first at {group.Min(c => c.OpenedAt):T}");
                }
            }
        }
        catch (Exception ex)
        {
            info.Error = $"Could not read the database details: {ex.Message}";
        }

        return info;
    }
}
