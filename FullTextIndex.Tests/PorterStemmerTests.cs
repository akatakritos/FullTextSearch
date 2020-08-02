using FullTextIndex.Core;
using NFluent;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FullTextIndex.Tests
{
    public class PorterStemmerTests
    {
       

        [Theory]
        [InlineData("caresses", "caress")]
        [InlineData("ponies", "poni")]
        [InlineData("ties", "ti")]
        [InlineData("caress", "caress")]
        [InlineData("cats", "cat")]
        public void Step1A(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            var result = stemmer.Stem(input);
            Check.That(result).IsEqualTo(stem);
        }

        [Theory]
        [InlineData("feed", "feed")]
        [InlineData("agreed", "agree")]
        [InlineData("plastered", "plaster")]
        [InlineData("bled", "bled")]
        [InlineData("motoring", "motor")]
        [InlineData("sing", "sing")]
        public void Step1B(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            Check.That(stemmer.Stem(input)).IsEqualTo(stem);
        }
    }
}
