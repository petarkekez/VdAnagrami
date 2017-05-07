using SQLite;

namespace VdAnagrami.Model
{
    public class Anagram
    {
        public Anagram()
        {
        }

        public Anagram(string question, string answer)
        {
            ID = 0;
            Answer = answer;
            Question = question;
            Solved = false;
        }


        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Solved { get; set; }
    }
}
