using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VdAnagrami
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnagramPage : ContentPage
    {
        private string AnagramQuestion; // { get; set; }
        private string AnagramAnswer; // { get; set; }
        private int anagramId;

        //public AnagramPage()
        //{
        //    InitializeComponent();

        //    InitializeForm();
        //}

        public AnagramPage(int id)
        {
            InitializeComponent();

            Title = "Anagram";

            anagramId = id;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var anagram = await App.Database.GetAnagramAsync(anagramId);

            AnagramQuestion = anagram.Question;
            AnagramAnswer = anagram.Answer.ToUpper();
            lblAnagramAnswer.Text = String.Empty;

            InitializeForm();
        }


        //public AnagramPage(string anagramQuestion, string anagramAnswer)
        //{
        //    InitializeComponent();

        //    AnagramQuestion = anagramQuestion;
        //    AnagramAnswer = anagramAnswer.ToUpper();

        //    InitializeForm();

        //}

        private void InitializeForm()
        {
            lblAnagramQuestion.Text = AnagramQuestion;

            List<string> anagramLetters = new List<string>();
            foreach (var character in AnagramQuestion)
            {
                if (Char.IsLetter(character))
                {
                    anagramLetters.Add(character.ToString());
                }
            }

            int columnsCount = gridAnagramKeyboardLetters.ColumnDefinitions.Count;
            int rowsCount = (anagramLetters.Count + columnsCount -1)/columnsCount;

            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                var rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);

                gridAnagramKeyboardLetters.RowDefinitions.Add(rowDefinition);
            }
            
            for (int letterIndex = 0; letterIndex < anagramLetters.Count; letterIndex++)
            {
                var btnLetter = new Button();
                btnLetter.Text = anagramLetters[letterIndex].ToUpper();
                btnLetter.Clicked += Keyboard_Clicked;

                gridAnagramKeyboardLetters.Children.Add(btnLetter, letterIndex % columnsCount, letterIndex / columnsCount);
            }
        }

        private async void Keyboard_Clicked(object sender, EventArgs e)
        {
            var btnKeyboard = (Button)sender;
            if (btnKeyboard.Text == "Space")
            {
                lblAnagramAnswer.Text = lblAnagramAnswer.Text + " ";
            }
            else if (btnKeyboard.Text == "<-")
            {
                if (lblAnagramAnswer.Text.Length > 0)
                {
                    string lastChar = lblAnagramAnswer.Text[lblAnagramAnswer.Text.Length - 1].ToString().ToLower();
                    if (lastChar != " ")
                    {
                        foreach (var item in gridAnagramKeyboardLetters.Children)
                        {
                            if (item is Button)
                            {
                                var key = (Button)item;
                                if (key.Text.ToLower() == lastChar && key.IsEnabled == false)
                                {
                                    key.IsEnabled = true;
                                    break;
                                }
                            }
                        }
                    }


                    lblAnagramAnswer.Text = lblAnagramAnswer.Text.Substring(0, lblAnagramAnswer.Text.Length - 1);
                }
            }
            else
            {
                lblAnagramAnswer.Text = lblAnagramAnswer.Text + btnKeyboard.Text.ToUpper();
                btnKeyboard.IsEnabled = false;

                var tempAnswer = lblAnagramAnswer.Text.Trim();
                while (tempAnswer.IndexOf("  ") > -1)
                    tempAnswer = tempAnswer.Replace("  ", " ");

                if (AnagramAnswer == tempAnswer)
                {
                    var anagram = await App.Database.GetAnagramAsync(anagramId);
                    anagram.Solved = true;
                    await App.Database.SaveItemAsync(anagram);

                    await DisplayAlert("Točno", "Ispravano ste odgovorili!", "OK");

                    await Navigation.PopAsync();
                }
            }
        }
    }
}
