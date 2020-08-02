using FullTextIndex.Core;
using NFluent;
using System;
using Xunit;

namespace FullTextIndex.Tests
{
    public class SimpleTokenizerTests
    {
        [Fact]
        public void GetTokens_KeepsWords()
        {
            var input = "A donut on a glass plate. Only the donuts.";
            var subject = new SimpleTokenizer();
            var tokens = subject.GetTokens(input);

            Check.That(tokens).ContainsExactly(
                "A", "donut", "on", "a", "glass", "plate", "Only", "the", "donuts");

        }

        [Fact]
        public void GetTokens_SingleWord()
        {
            var subject = new SimpleTokenizer();
            var tokens = subject.GetTokens("cat");
            Check.That(tokens).ContainsExactly("cat");
        }
       
    }
}
