using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace VdAnagrami.Data
{
    public class ConfigurationRepository
    {
        readonly SQLiteAsyncConnection database;

        public ConfigurationRepository(SQLiteAsyncConnection dbConnection)
        {
            database = dbConnection;
        }
    }
}
