using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FullTextIndex
{
    public class ContainsSearch
    {
        public ContainsSearch(List<WikipediaEntry> entries)
        {
            Entries = entries;
        }

        public List<WikipediaEntry> Entries { get; }

        public IEnumerable<WikipediaEntry> Search(string term)
        {
            var regex = new Regex("\\b" + term + "\\b", RegexOptions.IgnoreCase);
            return Entries.Where(e => regex.IsMatch(e.Abstract));
        }
    }
}
