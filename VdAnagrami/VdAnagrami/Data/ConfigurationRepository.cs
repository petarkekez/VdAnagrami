using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using VdAnagrami.Model;

namespace VdAnagrami.Data
{
    public class ConfigurationRepository
    {
        private const string databaseVersionKey = "DatabaseVersion";
        private const int databaseVersionNotInsertedValue = -1;
        readonly SQLiteAsyncConnection database;

        public ConfigurationRepository(SQLiteAsyncConnection dbConnection)
        {
            database = dbConnection;
        }

        public async Task<int> GetCurrentVersion()
        {
            //var configDatabaseVersion = await database.Table<Config>().Where(i => i.Key == databaseVersionKey).FirstOrDefaultAsync();
            var configDatabaseVersion = database.Table<Config>().ToListAsync().Result.Where(i => i.Key == databaseVersionKey).FirstOrDefault();
            if (configDatabaseVersion == null)
                return databaseVersionNotInsertedValue;
            else
                return int.Parse(configDatabaseVersion.Value);
        }

        public async Task SetVersion(int databaseVersion)
        {
            //var configDatabaseVersion = await database.Table<Config>().Where(i => i.Key == databaseVersionKey).FirstOrDefaultAsync();
            var configDatabaseVersion = database.Table<Config>().ToListAsync().Result.Where(i => i.Key == databaseVersionKey).FirstOrDefault();

            if (configDatabaseVersion == null)
            {
                var newConfigDatabaseVersion = new Config();
                newConfigDatabaseVersion.Key = databaseVersionKey;
                newConfigDatabaseVersion.Value = databaseVersion.ToString();
                database.InsertAsync(newConfigDatabaseVersion).Wait();
            }
            else
            {
                configDatabaseVersion.Value = databaseVersion.ToString();
                database.UpdateAsync(configDatabaseVersion).Wait();
            }
        }
    }
}
