using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdAnagrami.Data;
using Xamarin.Forms;

namespace VdAnagrami
{
    public partial class App : Application
    {

        static AnagramDatabase database;

        public App()
        {
            InitializeComponent();

            //InitialDatabase().Wait();

            MainPage = new NavigationPage(new VdAnagrami.MainPage())
            {
                BarBackgroundColor = VdAnagrami.Resources.Colors.PageBackgroundHighlight,
                BarTextColor = VdAnagrami.Resources.Colors.Text,
                BackgroundColor = VdAnagrami.Resources.Colors.PageBackground
            };
        }

        public static AnagramDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new AnagramDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return database;
            }
        }

        protected override async void OnStart()
        {
            //await InitialDatabase();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async Task InitialDatabase()
        {
            if (database == null)
            {
                database = new AnagramDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
            }

            var anagrams = await App.Database.Anagrams.GetAsync();

            if (anagrams.Count == 0)
            {
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Ep: Trka zeke", "Petar Kekez"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Stavit vino na noćnik", "Ivan Konstantinović"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Vaše tone", "Ante Ševo"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Klub račić", "Luka Brčić"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Vinil bića", "Ivan Bilić"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Od tamne fraze", "Frane Domazet"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("I kasom drljam..", "Damir Moskalj"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Tata! On nama lane!", "Antonela Matana"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Povećani vladar", "Andrea Pavlović"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Naći baru", "Ana Burić"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Ta mama dozrije", "Marija Domazet"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Ovakva noću pali!", "Paula Novaković"));
                await App.Database.Anagrams.InsertAsync(new Model.Anagram("Naći mali dren", "Marina Lendić"));

                //await App.Database.SaveItemAsync(new Model.Anagram("Štetna caka", "Ante Caktaš"));
                //await App.Database.SaveItemAsync(new Model.Anagram("Friški pilot", "filip krišto"));
                //await App.Database.SaveItemAsync(new Model.Anagram("Nadlaj i notne", "ante doljanin"));
                //await App.Database.SaveItemAsync(new Model.Anagram("Dojiš ćorak", "joško radić"));
                //await App.Database.SaveItemAsync(new Model.Anagram("Mlatim kotao", "toma mlikota"));
                //await App.Database.SaveItemAsync(new Model.Anagram("Rak lipe noći", "nikola perić"));
            }
        }
    }
}
