using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace FullTextIndex.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SingleWordSearches>();
        }
    }

    public class SingleWordSearches
    {
        private ContainsSearch containsSearcher;
        [GlobalSetup]
        public void Setup()
        {
            var entries = EntryReader.ReadDump(@"C:\Users\matt.burke.POINT\Downloads\enwiki-latest-abstract1.xml\enwiki-latest-abstract1.xml").ToList();

            containsSearcher = new ContainsSearch(entries);
        }

        [Benchmark(Description = "Contains Search")]
        public void ContainsSearch()
        {
            containsSearcher.Search("cat").Count();
        }
    }
}
