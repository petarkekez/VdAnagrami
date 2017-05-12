using SQLite;

namespace VdAnagrami.Model
{
    public class Config
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
