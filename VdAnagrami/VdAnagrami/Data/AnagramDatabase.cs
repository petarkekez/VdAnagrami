using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public AnagramRepository Anagrams { get { return anagram; } }
        public ConfigurationRepository Configuration { get { return configuration; } }


        public AnagramDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
        }

        public void InitializeDatabase()
        {
            CreateDatabase();

            InitializeRepositories();

            Migrations();
        }

        private void CreateDatabase()
        {
            var bla = database.CreateTableAsync<Anagram>().Result;
            foreach (var item in bla.Results)
            {
                Debug.WriteLine(item.ToString());
            }
            var bla2 = database.CreateTableAsync<Config>().Result;
        }

        private void InitializeRepositories()
        {
            anagram = new AnagramRepository(database);
            configuration = new ConfigurationRepository(database);
        }


        private void Migrations()
        {
            var databaseVersionUpdated = false;
            var databaseVersion = Configuration.GetCurrentVersion().Result;
            if (databaseVersion == -1)
            {
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Ep: Trka zeke", "Petar Kekez")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Stavit vino na noćnik", "Ivan Konstantinović")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Vaše tone", "Ante Ševo")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Klub račić", "Luka Brčić")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Vinil bića", "Ivan Bilić")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Od tamne fraze", "Frane Domazet")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("I kasom drljam..", "Damir Moskalj")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Tata! On nama lane!", "Antonela Matana")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Povećani vladar", "Andrea Pavlović")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Naći baru", "Ana Burić")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Ta mama dozrije", "Marija Domazet")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Ovakva noću pali!", "Paula Novaković")).Wait();
                App.Database.Anagrams.InsertAsync(new Model.Anagram("Naći mali dren", "Marina Lendić")).Wait();

                databaseVersion = 1;
                databaseVersionUpdated = true;
            }

            if (databaseVersion == 1)
            {
                App.Database.Anagrams.UpdateAnagramQuestion("Ep: Trka zeke", "bla bla").Wait();
                databaseVersion = 2;
                databaseVersionUpdated = true;
            }

            if (databaseVersion == 2)
            {
                App.Database.Anagrams.UpdateAnagramQuestion("bla bla", "Ep: Trka zeke").Wait();
                databaseVersion = 3;
                databaseVersionUpdated = true;
            }

            if (databaseVersionUpdated)
                Configuration.SetVersion(databaseVersion).Wait();
        }




        //public Task<int> DeleteItemAsync(TodoItem item)
        //{
        //    return database.DeleteAsync(item);
        //}
    }
}
