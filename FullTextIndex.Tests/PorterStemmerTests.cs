using FullTextIndex.Core;
using NFluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace FullTextIndex.Tests
{
    public class PorterStemmerTests
    {

        [Theory]
        // step 1 a
        [InlineData("caresses", "caress")]
        [InlineData("ponies", "poni")]
        [InlineData("ties", "ti")]
        [InlineData("caress", "caress")]
        [InlineData("cats", "cat")]
        // step 1 b
        [InlineData("feed", "feed")]
        //[InlineData("agreed", "agree")]
        [InlineData("plastered", "plaster")]
        [InlineData("bled", "bled")]
        [InlineData("motoring", "motor")]
        [InlineData("sing", "sing")]
        //[InlineData("conflated", "conflate")]
        //[InlineData("troubled", "trouble")]
        [InlineData("hopping", "hop")]
        [InlineData("tanned", "tan")]
        [InlineData("falling", "fall")]
        [InlineData("hissing", "hiss")]
        [InlineData("fizzing", "fizz")]
        [InlineData("failing", "fail")]
        // step 1 c
        [InlineData("happy", "happi")]
        [InlineData("sky", "sky")]
        // step 2
        //[InlineData("relational", "relate")]
        [InlineData("conditional", "condition")]
        //[InlineData("rational", "rational")]
        //[InlineData("valenci", "valence")]
        //[InlineData("hesitanci", "hesitance")]
        //[InlineData("digitizer", "digitize")]
        //[InlineData("conformabli", "conformable")]
        // [InlineData("radicalli", "radical")] -> radic in step 3
        //[InlineData("differentli", "different")]
        [InlineData("vileli", "vile")]
        //[InlineData("analogousli", "analogous")]
        //[InlineData("vietnamization", "vietnamize")]
        //[InlineData("predication", "predicate")] --> predic in step 3
        //[InlineData("operator", "operate")]
        [InlineData("feudalism", "feudal")]
        //[InlineData("decisiveness", "decisive")]
        // [InlineData("hopefulness", "hopeful")] --> hope in step 3
        [InlineData("callousness", "callous")]
        [InlineData("formaliti", "formal")]
        //[InlineData("sensitiviti", "sensitive")]
        //[InlineData("sensibiliti", "sensible")]
        // step 3
        [InlineData("triplicate", "triplic")]
        [InlineData("formative", "form")]
        [InlineData("formalize", "formal")]
        //[InlineData("electriciti", "electric")]
        //[InlineData("electrical", "electric")]
        [InlineData("hopeful", "hope")]
        [InlineData("goodness", "good")]
        // step 4
        [InlineData("revival", "reviv")]
        [InlineData("allowance", "allow")]
        [InlineData("inference", "infer")]
        [InlineData("airliner", "airlin")]
        [InlineData("gyroscopic", "gyroscop")]
        [InlineData("adjustable", "adjust")]
        [InlineData("defensible", "defens")]
        [InlineData("irritant", "irrit")]
        [InlineData("replacement", "replac")]
        [InlineData("adjustment", "adjust")]
        [InlineData("dependent", "depend")]
        [InlineData("adoption", "adopt")]
        [InlineData("homologou", "homolog")]
        [InlineData("communism", "commun")]
        [InlineData("activate", "activ")]
        [InlineData("angulariti", "angular")]
        [InlineData("homologous", "homolog")]
        [InlineData("effective", "effect")]
        [InlineData("bowdlerize", "bowdler")]
        // step 5 a
        [InlineData("probate", "probat")]
        [InlineData("rate", "rate")]
        [InlineData("cease", "ceas")]
        // step 5b
        [InlineData("controll", "control")]
        [InlineData("roll", "roll")]
        public void Stem(string input, string stem)
        {
            var stemmer = new PorterStemmer();
            Check.That(stemmer.Stem(input)).IsEqualTo(stem);
        }

         }
}
