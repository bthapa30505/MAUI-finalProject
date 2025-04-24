using SQLite;
using MAUI_finalProject.Models;

namespace MAUI_finalProject.Services
{
    public class DBConnection
    {
        private readonly SQLiteAsyncConnection _database;

        public DBConnection(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LocationInfo>().Wait();
        }

        public Task<int> SaveLocationAsync(LocationInfo location)
        {
            return _database.InsertAsync(location);
        }

        public Task<List<LocationInfo>> GetLocationsAsync()
        {
            return _database.Table<LocationInfo>().ToListAsync();
        }

        public Task<int> DeleteAllAsync()
        {
            return _database.DeleteAllAsync<LocationInfo>();
        }
    }
}
