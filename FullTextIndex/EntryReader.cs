using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FullTextIndex
{
    public static class EntryReader
    {
        public static IEnumerable<WikipediaEntry> ReadDump(string filename)
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
}
