using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdAnagrami.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VdAnagrami
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnagramPage : ContentPage
    {
        private const double coursorHiddenOpacity = 0;
        private const double coursorVisibleOpacity = 1;
        private const string separator = "|";
        private const string spaceLabelText = " ";
        private const double spaceLabelWidth = 10;

        private string AnagramQuestion; // { get; set; }
        private string AnagramAnswer; // { get; set; }
        private int anagramId;
        private bool isSolved;

        public AnagramPage(int id)
        {
            InitializeComponent();

            Title = "Anagram";

            anagramId = id;
            AddInitialInputField();

            var anagram = App.Database.Anagrams.GetAsync(anagramId).Result;

            isSolved = anagram.Solved;
            AnagramQuestion = anagram.Question;
            AnagramAnswer = anagram.Answer.ToUpper();

            InitializeForm();

            if (anagram.Solved)
            {
                lblAnagramAnswerHead.IsVisible = true;
                lblAnagramAnswer.Text = AnagramAnswer;
                lblAnagramAnswer.IsVisible = true;
            }
        }

        private void AddInitialInputField()
        {
            var initialLabel = new Label() { Text = spaceLabelText, Margin = 0, TextColor = VdAnagrami.Resources.Colors.TextHighlight };
            initialLabel.WidthRequest = spaceLabelWidth;
            AnswerList.Children.Add(initialLabel);

            var initialCursor = new Label() { Text = separator, Margin = 0, TextColor = VdAnagrami.Resources.Colors.TextHighlight };
            initialCursor.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command<Label>((Label label) => CursorClicked(label)),
                CommandParameter = initialCursor,
                NumberOfTapsRequired = 1
            });

            AnswerList.Children.Add(initialCursor);

            var characterLabel = new Label() { Text = spaceLabelText, Margin = 0, TextColor = VdAnagrami.Resources.Colors.TextHighlight };
            characterLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command<Label>((Label label) => AnswerCharacterClicked(label)),
                CommandParameter = characterLabel,
                NumberOfTapsRequired = 1
            });
            if (characterLabel.Text == spaceLabelText)
                characterLabel.WidthRequest = spaceLabelWidth;
            AnswerList.Children.Add(characterLabel);
        }
        
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
                btnLetter.TextColor = VdAnagrami.Resources.Colors.TextHighlight;
                btnLetter.BackgroundColor = VdAnagrami.Resources.Colors.ButtonBackground;

                gridAnagramKeyboardLetters.Children.Add(btnLetter, letterIndex % columnsCount, letterIndex / columnsCount);
            }
        }

        private async void Keyboard_Clicked(object sender, EventArgs e)
        {
            var btnKeyboard = (Button)sender;
            if (btnKeyboard.Text == "Space")
            {
                AddCharacterToAnswer(spaceLabelText);

                await CheckAnswer();
            }
            else if (btnKeyboard.Text == "<-")
            {
                var cursorIndex = GetCurrentCursorIndex();
                if (cursorIndex > 1)
                {
                    var removedLetter = ((Label)AnswerList.Children[cursorIndex - 1]).Text;
                    foreach (Button item in gridAnagramKeyboardLetters.Children)
                        if (item.IsEnabled == false && item.Text == removedLetter)
                        { 
                            item.IsEnabled = true;
                            break;
                        }

                    AnswerList.Children.RemoveAt(cursorIndex - 1);
                    AnswerList.Children.RemoveAt(cursorIndex - 2);
                }
            }
            else
            {
                AddCharacterToAnswer(btnKeyboard.Text);
                btnKeyboard.IsEnabled = false;
                
                await CheckAnswer();
            }
        }

        private async Task CheckAnswer()
        {
            if (isSolved == false)
            {
                var tempAnswer = GetCurrentAnswer();

                if (AnagramAnswer == tempAnswer)
                {
                    var anagram = await App.Database.Anagrams.GetAsync(anagramId);
                    anagram.Solved = true;
                    await App.Database.Anagrams.UpdateAsync(anagram);

                    await DisplayAlert("Točno odgovor", "\"" + AnagramAnswer + "\"", "OK");

                    await Navigation.PopAsync();
                }
            }
        }

        private int GetCurrentCursorIndex()
        {
            for (int i = 0; i < AnswerList.Children.Count; i++)
            {
                var currentLabel = (Label)AnswerList.Children[i];
                if (currentLabel.Text == separator && currentLabel.Opacity == coursorVisibleOpacity)
                    return i;
            }
            return -1;
        }

        private string GetCurrentAnswer()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in AnswerList.Children)
            {
                if (item is Label)
                {
                    var currentLabel = (Label)item;

                    if (currentLabel.Text == separator)
                        continue;
                    else if (currentLabel.Text == spaceLabelText)
                        sb.Append(" ");
                    else
                        sb.Append(currentLabel.Text);
                }
            }

            return sb.ToString().Trim();
        }

        private void AddCharacterToAnswer(string v)
        {
            for (int i = 0; i < AnswerList.Children.Count; i++)
            {
                var currentCharacter = (Label)AnswerList.Children[i];
                if (currentCharacter.Text == separator && currentCharacter.Opacity == coursorVisibleOpacity)
                {
                    currentCharacter.Opacity = coursorHiddenOpacity;

                    var characterLabel = new Label() { Text = v, Margin = 0, TextColor = VdAnagrami.Resources.Colors.TextHighlight };
                    characterLabel.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command<Label>((Label label) => AnswerCharacterClicked(label)),
                        CommandParameter = characterLabel,
                        NumberOfTapsRequired = 1
                    });
                    if (characterLabel.Text == spaceLabelText)
                        characterLabel.WidthRequest = spaceLabelWidth;
                    AnswerList.Children.Insert(i+1, characterLabel);
                    

                    var cursorLabel = new Label();
                    cursorLabel.Text = separator;
                    cursorLabel.Opacity = coursorVisibleOpacity;
                    cursorLabel.Margin = 0;
                    cursorLabel.TextColor = VdAnagrami.Resources.Colors.TextHighlight;
                    cursorLabel.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command<Label>((Label label) =>  CursorClicked(label)),
                        CommandParameter = cursorLabel,
                        NumberOfTapsRequired = 1
                    });

                    AnswerList.Children.Insert(i + 2, cursorLabel);
                    i += 2;
                }
            }
        }

        private void AnswerCharacterClicked(Label answerCharacterLabel)
        {
            for (int i = 0; i < AnswerList.Children.Count; i++)
            {
                var currentCharacter = (Label)AnswerList.Children[i];
                if (currentCharacter == answerCharacterLabel)
                {
                    if (i == 0)
                        break;
                    else
                        AnswerList.Children[i - 1].Opacity = coursorVisibleOpacity;
                }
                else if (currentCharacter.Text == separator)
                {
                    AnswerList.Children[i].Opacity = coursorHiddenOpacity;
                }
            }
        }

        private void CursorClicked(Label label)
        {
            for (int i = 0; i < AnswerList.Children.Count; i++)
            {
                var currentCharacter = (Label)AnswerList.Children[i];
                if (currentCharacter.Text == separator)
                {
                    AnswerList.Children[i].Opacity = coursorHiddenOpacity;
                }
            }
            label.Opacity = coursorVisibleOpacity;
        }
        
    }
}
