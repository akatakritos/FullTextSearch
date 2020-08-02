using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FullTextIndex.Core
{
    public class SearchResult
    {
        public string DocumentId { get; }

        public SearchResult(string documentId)
        {
            DocumentId = documentId;
        }
    }

    public class InvertedIndex
    {
        SimpleTokenizer tokenizer = new SimpleTokenizer();
        Dictionary<string, List<string>> index;
        PorterStemmer stemmer = new PorterStemmer();

        public int DocumentCount { get; private set; } = 0;
        public int TermCount => index.Count;

        public InvertedIndex()
        {
            index = new Dictionary<string, List<string>>();
        }

        public void Index(string documentId, string content)
        {
            var tokens = tokenizer.GetTokens(content)
                .Select(t => t.ToLowerInvariant())
                .Select(t => stemmer.Stem(t));

            var uniqueTokens = new HashSet<string>(tokens);
            foreach (var token in uniqueTokens)
            {
                Add(token, documentId);
            }

            DocumentCount++;
        }

        private void Add(string token, string documentId)
        {
            if (index.ContainsKey(token))
            {
                index[token].Add(documentId);
                return;
            }

            var list = new List<string>();
            list.Add(documentId);
            index[token] = list;
        }

        public IEnumerable<SearchResult> Search(string term)
        {
            term = stemmer.Stem(term.ToLowerInvariant());

            if (index.ContainsKey(term))
            {
                return index[term].Select(documentId => new SearchResult(documentId));
            }

            return Enumerable.Empty<SearchResult>();
        }
    }
}
