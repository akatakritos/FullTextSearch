﻿using FullTextIndex.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace FullTextIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            //var sw = Stopwatch.StartNew();
            //var entries = EntryReader.ReadDump(@"C:\Users\matt.burke.POINT\Downloads\enwiki-latest-abstract1.xml\enwiki-latest-abstract1.xml").ToList();
            //sw.Stop();
            //Console.WriteLine($"Read {entries.Count} in {sw.ElapsedMilliseconds}ms");

            //sw.Restart();
            //var invertedIndex = new InvertedIndex(entries.Count);
            //foreach (var entry in entries)
            //    invertedIndex.Index(entry);
            //sw.Stop();
            //Console.WriteLine($"Created inverted index of {entries.Count} items in {sw.ElapsedMilliseconds}ms");

            //var containsSearch = new ContainsSearch(entries);
            //sw.Restart();
            //var matches = containsSearch.Search("cat").Count();
            //sw.Stop();

            //foreach (var match in containsSearch.Search("cat"))
            //{
            //    Console.WriteLine(match.Abstract);
            //}
            //Console.WriteLine($"Contains - found {matches} matches in {sw.ElapsedMilliseconds}ms");

            //sw.Restart();
            //matches = invertedIndex.Search("cat").Count();
            //sw.Stop();

            //foreach (var match in invertedIndex.Search("cat"))
            //    Console.WriteLine(match.Abstract);

            //Console.WriteLine($"Index - found {matches} matches in {sw.ElapsedMilliseconds}ms");

            var lines = File.ReadAllLines("diff.txt");
            var regex = new Regex(@"^(\w+)\s+(\w+)$");
            var stemmer = new PorterStemmer();
            int correct = 0;
            int total = 0;

            foreach (var line in lines)
            {
                total++;
                var mc = regex.Match(line);
                var input = mc.Groups[1].Value;
                var expected = mc.Groups[2].Value;

                var result = stemmer.Stem(input);

                if (result == expected)
                {
                    correct++;
                    //Console.WriteLine($"+ {input} -> {expected}");
                }
                else
                {
                    Console.WriteLine($"- {input} -> {expected}. Got {result}");
                } 

            }

            Console.WriteLine($"Results: {correct * 100.0 / total}% ({correct}/{total})");

        }


    }
}
