using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdAnagrami.Model;

namespace VdAnagrami.Data
{
    public class AnagramDatabase
    {
        readonly SQLiteAsyncConnection database;
        private AnagramRepository anagram;
        private ConfigurationRepository configuration;

        public AnagramDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Anagram>().Wait();
            database.CreateTableAsync<Config>().Wait();

            anagram = new AnagramRepository(database);
            configuration = new ConfigurationRepository(database);
        }

        public AnagramRepository Anagrams { get { return anagram; } }
        public ConfigurationRepository Configuration { get { return configuration; } }

        public void CreateDatabase()
        {
            database.CreateTableAsync<Anagram>().Wait();
            database.CreateTableAsync<Config>().Wait();
        }


        

        //public Task<int> DeleteItemAsync(TodoItem item)
        //{
        //    return database.DeleteAsync(item);
        //}
    }
}
