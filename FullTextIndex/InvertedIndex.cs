﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FullTextIndex
{
    public class InvertedIndex
    {
        SimpleTokenizer tokenizer = new SimpleTokenizer();
        List<WikipediaEntry> entries;
        Dictionary<string, List<int>> index;

        public InvertedIndex(int initialCapacity)
        {
            entries = new List<WikipediaEntry>(initialCapacity);
            index = new Dictionary<string, List<int>>();
        }

        public void Index(WikipediaEntry entry)
        {
            var tokens = tokenizer.GetTokens(entry.Abstract).Select(t => t.ToLowerInvariant());
            var uniqueTokens = new HashSet<string>(tokens);
            foreach (var token in uniqueTokens)
            {
                Add(token, entries.Count);
            }

            entries.Add(entry);
        }

        private void Add(string token, int documentIndex)
        {
            if (index.ContainsKey(token))
            {
                index[token].Add(documentIndex);
                return;
            }

            var list = new List<int>();
            list.Add(documentIndex);
            index[token] = list;
        }

        public IEnumerable<WikipediaEntry> Search(string term)
        {
            if (index.ContainsKey(term))
            {
                return index[term].Select(docIndex => entries[docIndex]);
            }

            return Enumerable.Empty<WikipediaEntry>();
        }
    }
}