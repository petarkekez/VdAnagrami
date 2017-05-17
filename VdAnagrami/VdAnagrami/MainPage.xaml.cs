using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VdAnagrami
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Title = "VD Anagrami";
        }
        
        protected override async void OnAppearing()
        {
            //base.OnAppearing();
            
            await InitialDatabase();

            await InitializeAnagrams();
        }

        private async Task InitializeAnagrams()
        {
            var anagrams = await App.Database.Anagrams.GetAsync();
            
            var columnCount = gridAnagrams.ColumnDefinitions.Count;
            var rowCount = gridAnagrams.RowDefinitions.Count;
            if (rowCount == 0)
            {
                rowCount = (anagrams.Count + columnCount - 1) / columnCount;

                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var rowDefinition = new RowDefinition();
                    rowDefinition.Height = new GridLength(1, GridUnitType.Star);

                    gridAnagrams.RowDefinitions.Add(rowDefinition);
                }
            }

            gridAnagrams.Children.Clear();
            int solvedCount = 0;
            for (int i = 0; i < anagrams.Count; i++)
            {
                Image newImage = new Image();

                if (anagrams[i].Solved == false)
                    newImage.Source = ImageSource.FromResource("VdAnagrami.Images.Question.png");
                else
                {
                    solvedCount++;
                    newImage.Source = ImageSource.FromResource("VdAnagrami.Images.Solved.png");
                }
                
                newImage.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command<int>((int id) =>
                    {
                        Navigation.PushAsync(new AnagramPage(id));
                    }),
                    CommandParameter = anagrams[i].ID,
                    NumberOfTapsRequired = 1
                });
                 
                gridAnagrams.Children.Add(newImage, i % columnCount, i / columnCount);
            }
            lblSolved.Text = "Riješeno " + solvedCount + "/" + anagrams.Count;
        }

        private async Task InitialDatabase()
        {
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
