using FullTextIndex.Core;
using NFluent;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FullTextIndex.Tests
{
    public class WordTests
    {
        [Theory]
        [InlineData("carresses", "sses", true)]
        [InlineData("carress", "sses", false)]
        public void EndsWith_Works(string input, string suffix, bool expected)
        {
            var word = new Word(input);
            Check.That(word.EndsWith(suffix)).IsEqualTo(expected);
        }

        [Theory]
        [InlineData("caresses", "sses", "ss", "caress")]
        public void ReplaceSuffix_Works(string input, string suffix, string replacement, string expected)
        {
            var word = new Word(input);
            word.ReplaceSuffix(suffix, replacement);
            Check.That(word.Value).IsEqualTo(expected);
        }

        [Theory]
        [InlineData("toy", 0, true)]
        [InlineData("toy", 1, false)]
        [InlineData("toy", 2, true)]
        [InlineData("syzygy", 0, true)]
        [InlineData("syzygy", 1, false)]
        [InlineData("syzygy", 2, true)]
        [InlineData("syzygy", 3, false)]
        [InlineData("syzygy", 4, true)]
        [InlineData("syzygy", 5, false)]
        public void IsConsonent(string input, int index, bool isConsonent)
        {
            var word = new Word(input);
            var result = word.IsConsonant(index);
            Check.That(result).IsEqualTo(isConsonent);
        }

        [Theory]
        [InlineData("")]       // null word
        [InlineData("tr")]     // C
        [InlineData("ee")]     // V
        [InlineData("tree")]   // CV
        [InlineData("y")]      // C
        [InlineData("by")]     // C
        public void Measure_0(string input)
        {
            var word = new Word(input);
            Check.That(word.Measure).IsEqualTo(0);
        }

        [Theory]
        [InlineData("trouble")]  // C(VC)V
        [InlineData("oats")]     // (VC)
        [InlineData("trees")]    // C(VC)
        [InlineData("ivy")]      // (VC)V
        public void Measure_1(string input)
        {
            var word = new Word(input);
            Check.That(word.Measure).IsEqualTo(1);
        }


        [Theory]
        [InlineData("troubles")]    // C(VC)(VC)
        [InlineData("private")]     // C(VC)(VC)V
        [InlineData("oaten")]       // (VC)(VC)
        [InlineData("orrery")]      // (VC)(VC)V
        public void Measure_2(string input)
        {
            var word = new Word(input);
            Check.That(word.Measure).IsEqualTo(2);
        }

    }
}
