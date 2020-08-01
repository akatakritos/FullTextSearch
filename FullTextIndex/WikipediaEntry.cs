namespace FullTextIndex
{
    public class WikipediaEntry
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Abstract { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Url} - {Abstract}";
        }
    }
}
