using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FullTextIndex.Core
{
    public class SearchResult
    {
        public string DocumentId { get; }
        public float Score { get; }

        public SearchResult(string documentId, float score)
        {
            DocumentId = documentId;
            Score = score;
        }
    }

    public class IndexState
    {
        public Dictionary<string, Dictionary<string, MatchData>> Index { get; set; }
        public Dictionary<string, DocumentData> DocumentData { get; set; }
    }


    public class MatchData
    {
    }

    public class DocumentData
    {
        public int Length { get; set; } = 0;
        public Dictionary<string, int> TermFrequencies { get; } = new Dictionary<string, int>();
        public TermVector Vector { get; } = new TermVector();
    }


    public class InvertedIndex
    {
        Dictionary<string, Dictionary<string, MatchData>> index;
        Dictionary<string, DocumentData> documentData;

        SimpleTokenizer tokenizer = new SimpleTokenizer();
        PorterStemmer stemmer = new PorterStemmer();
        EnglishStopWordsFilter stopWordsFilter = new EnglishStopWordsFilter();

        public int DocumentCount => documentData.Keys.Count;
        public int TermCount => index.Count;


        public InvertedIndex()
        {
            index = new Dictionary<string, Dictionary<string, MatchData>>();
            documentData = new Dictionary<string, DocumentData>();
        }

        internal InvertedIndex(IndexState state)
        {
            index = state.Index;
            documentData = state.DocumentData;
        }

        internal IndexState GetStateForSerialization()
        {
            return new IndexState
            {
                Index = index,
                DocumentData = documentData
            };
        }

        public void Index(string documentId, string content)
        {
            var tokens = tokenizer.GetTokens(content.ToLowerInvariant())
                .Where(t => !stopWordsFilter.IsStopWord(t))
                .Select(t => stemmer.Stem(t));

            if (!documentData.ContainsKey(documentId))
                documentData[documentId] = new DocumentData();

            var doc = documentData[documentId];

            foreach (var term in tokens)
            {

                if (!doc.TermFrequencies.ContainsKey(term))
                    doc.TermFrequencies[term] = 0;

                doc.TermFrequencies[term]++;
                doc.Length++;

                Add(term, documentId);
            }
        }

        private void Add(string token, string documentId)
        {
            if (!index.ContainsKey(token))
                index[token] = new Dictionary<string, MatchData>();

            if (!index[token].ContainsKey(documentId))
                index[token][documentId] = new MatchData();

        }

        public IEnumerable<SearchResult> Search(string query)
        {
            var terms = tokenizer.GetTokens(query.ToLowerInvariant())
                   .Where(term => !stopWordsFilter.IsStopWord(term))
                   .Select(term => stemmer.Stem(term))
                   .Distinct()
                   .ToList();

            HashSet<string> documentIds = new HashSet<string>();

            var queryVector = new TermVector();
            foreach (var term in terms)
                queryVector.Add(term, 1); // 1 or boost


            foreach (var term in terms)
            {
                if (index.ContainsKey(term))
                {
                    foreach (var documentId in index[term].Keys)
                        documentIds.Add(documentId);
                }
            }

            return documentIds.Select(id =>
            {
                var doc = documentData[id];
                var score = doc.Vector.Similarity(queryVector);
                return new SearchResult(id, score);
            })
            .OrderByDescending(result => result.Score);
        }


        public void Commit()
        {
            var averageDocumentLength = documentData.Values.Average(d => d.Length);
            const float k1 = 1.2f;
            const float b = 0.75f;

            foreach (var documentId in documentData.Keys)
            {
                var doc = documentData[documentId];

                foreach (var term in doc.TermFrequencies.Keys)
                {
                    var tf = doc.TermFrequencies[term];
                    var documentsWithTerm = index[term].Keys.Count;
                    var idf = InverseDocumentFrequency.For(documentsWithTerm, DocumentCount);
                    var score = idf * ((k1 + 1) * tf) / (k1 * (1 - b + b * (doc.Length / averageDocumentLength)) + tf);

                    doc.Vector.Add(term, (float)score);
                }
            }
        }
    }
}
