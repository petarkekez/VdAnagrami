using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using VdAnagrami.Model;

namespace VdAnagrami.Data
{
    public class AnagramRepository
    {
        readonly SQLiteAsyncConnection database;

        public AnagramRepository(SQLiteAsyncConnection dbConnection)
        {
            database = dbConnection;
        }

        public Task<List<Anagram>> GetAsync()
        {
            return database.Table<Anagram>().ToListAsync();
        }

        //public Task<List<Anagram>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<Anagram>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public Task<Anagram> GetAsync(int id)
        {
            return database.Table<Anagram>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> InsertAsync(Anagram anagram)
        {
            return database.InsertAsync(anagram);
        }

        public Task<int> UpdateAsync(Anagram anagram)
        {
            return database.UpdateAsync(anagram);
        }
    }
}
