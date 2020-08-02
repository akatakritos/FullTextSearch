namespace FullTextIndex.Core
{
    public class WikipediaEntry
    {
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Abstract { get; set; } = null!;

        public override string ToString()
        {
            return $"{Title} - {Url} - {Abstract}";
        }
    }
}
