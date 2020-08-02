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
        [InlineData("conflated", "conflate")]
        [InlineData("troubled", "trouble")]
        [InlineData("hopping", "hop")]
        [InlineData("tanned", "tan")]
        [InlineData("falling", "fall")]
        [InlineData("hissing", "hiss")]
        [InlineData("fizzing", "fizz")]
        [InlineData("failing", "fail")]
        public void Step1B(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            Check.That(stemmer.Stem(input)).IsEqualTo(stem);
        }

        [Theory]
        [InlineData("happy", "happi")]
        [InlineData("sky", "sky")]
        public void Step1C(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            Check.That(stemmer.Stem(input)).IsEqualTo(stem);
        }

        [Theory]
        [InlineData("relational", "relate")]
        [InlineData("conditional", "condition")]
        [InlineData("rational", "rational")]
        [InlineData("valenci", "valence")]
        [InlineData("hesitanci", "hesitance")]
        [InlineData("digitizer", "digitize")]
        [InlineData("conformabli", "conformable")]
        // [InlineData("radicalli", "radical")] -> radic in step 3
        [InlineData("differentli", "different")]
        [InlineData("vileli", "vile")]
        [InlineData("analogousli", "analogous")]
        [InlineData("vietnamization", "vietnamize")]
        //[InlineData("predication", "predicate")] --> predic in step 3
        [InlineData("operator", "operate")]
        [InlineData("feudalism", "feudal")]
        [InlineData("decisiveness", "decisive")]
        // [InlineData("hopefulness", "hopeful")] --> hope in step 3
        [InlineData("callousness", "callous")]
        [InlineData("formaliti", "formal")]
        [InlineData("sensitiviti", "sensitive")]
        [InlineData("sensibiliti", "sensible")]
        public void Step2(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            Check.That(stemmer.Stem(input)).IsEqualTo(stem);
        }


        [Theory]
        [InlineData("triplicate", "triplic")]
        [InlineData("formative", "form")]
        [InlineData("formalize", "formal")]
        [InlineData("electriciti", "electric")]
        [InlineData("electrical", "electric")]
        [InlineData("hopeful", "hope")]
        [InlineData("goodness", "good")]
        public void Step3(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            Check.That(stemmer.Stem(input)).IsEqualTo(stem);
        }
    }
}
