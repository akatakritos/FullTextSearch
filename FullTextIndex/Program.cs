using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;

namespace FullTextIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            foreach(var entry in ReadDump(@"C:\Users\matt.burke.POINT\Downloads\enwiki-latest-abstract1.xml\enwiki-latest-abstract1.xml"))
            {
                Console.WriteLine(entry);
            }

        }

        private static IEnumerable<WikipediaEntry> ReadDump(string filename)
        {
            using (var reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "doc")
                    {
                        yield return ReadEntry(reader);
                    }
                }
            }
        }

        private static WikipediaEntry ReadEntry(XmlReader reader)
        {
            var entry = new WikipediaEntry();
            while (reader.Read())
            {

                if (reader.Name == "title")
                {
                    entry.Title = reader.ReadElementContentAsString();
                    continue;
                }


                if (reader.Name == "abstract")
                {
                    entry.Abstract = reader.ReadElementContentAsString();
                    continue;
                }

                if (reader.Name == "url")
                {
                    entry.Url = reader.ReadElementContentAsString();
                    continue;
                }

                if (reader.Name == "links")
                {
                    reader.Skip();
                }

                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "doc")
                {
                    return entry;
                }
            }

            throw new InvalidOperationException("reached end of document without closing an entry");
        }
    }

    class WikipediaEntry
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Abstract { get; set; }

        public override string ToString()
        {
            return $"{Title} - {Url} - {Abstract}";
        }
    }
}
