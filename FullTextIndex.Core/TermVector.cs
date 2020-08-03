using System;
using System.Collections.Generic;
using System.Linq;

namespace FullTextIndex.Core
{
    // Term vector
    public class TermVector
    {
        public Dictionary<string, float> Entries = new Dictionary<string, float>();

        public void Add(string term, float score)
        {
            Entries[term] = score;
        }

        public float Magnitude
        {
            get
            {
                var sumOfSquares = 0.0f;
                foreach (var value in Entries.Values)
                {
                    sumOfSquares += (value * value);
                }

                return (float)Math.Sqrt(sumOfSquares);
            }
        }

        public float this[string term] => Entries.ContainsKey(term) ? Entries[term] : 0;

        public float Dot(TermVector other)
        {
            // in Vector3 this would be ax * bx + ay * by + az * bz
            // sum the proeducts all all matching keys

            var dot = 0.0f;
            var sharedTerms = Entries.Keys.Intersect(other.Entries.Keys);
            foreach (var term in sharedTerms)
            {
                dot += (this[term] * other[term]);
            }

            return dot;
        }

        public float Similarity(TermVector other)
        {
            var magnitude = Magnitude;
            if (magnitude == 0)
                return 0;

            return Dot(other) / magnitude;
        }
    }
}
