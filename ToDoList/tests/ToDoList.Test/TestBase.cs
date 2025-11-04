namespace ToDoList.Test;

using Microsoft.Data.Sqlite;

public class TestBase
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    public static void CreateDatabase()
    {
        string? directory = Path.GetDirectoryName(DbPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }

    public static void DeleteDatabase()
    {
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }
}
