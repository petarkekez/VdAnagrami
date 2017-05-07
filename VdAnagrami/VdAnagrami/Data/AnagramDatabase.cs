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

        public AnagramDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Anagram>().Wait();
        }

        public Task<List<Anagram>> GetAnagramsAsync()
        {
            return database.Table<Anagram>().ToListAsync();
        }

        public Task<List<Anagram>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Anagram>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<Anagram> GetAnagramAsync(int id)
        {
            return database.Table<Anagram>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Anagram anagram)
        {
            if (anagram.ID != 0)
            {
                return database.UpdateAsync(anagram);
            }
            else
            {
                return database.InsertAsync(anagram);
            }
        }

        //public Task<int> DeleteItemAsync(TodoItem item)
        //{
        //    return database.DeleteAsync(item);
        //}
    }
}
