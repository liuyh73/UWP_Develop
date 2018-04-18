using List.Models;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;
using Windows.Storage;

namespace schedule.Services
{
    public static class ScheduleDB
    {
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "schedule.db");
        public static SQLiteConnection GetDbConnection()
        {
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            conn.CreateTable<ListItemDB>();
            return conn;
        }
    }
}
