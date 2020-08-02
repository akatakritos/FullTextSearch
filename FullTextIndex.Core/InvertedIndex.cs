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
                .Select(t => stemmer.Stem(t))
                .Distinct();

            foreach (var token in tokens)
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

        public IEnumerable<SearchResult> Search(string query)
        {
            var terms = tokenizer.GetTokens(query)
                   .Select(term => term.ToLowerInvariant())
                   .Select(term => stemmer.Stem(term))
                   .Distinct();

            HashSet<string> documentIds = new HashSet<string>();

            foreach(var term in terms)
            {
                if (index.ContainsKey(term))
                {
                    foreach (var documentId in index[term])
                        documentIds.Add(documentId);
                }

            }

            return documentIds.Select(id => new SearchResult(id));
        }
    }
}
