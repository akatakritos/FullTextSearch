using System.Collections.Generic;
using System.Text;

namespace FullTextIndex.Core
{
    public class SimpleTokenizer
    {
        public IEnumerable<string> GetTokens(string content)
        {
            var buffer = new StringBuilder();
            foreach (var c in content)
            {
                if (!char.IsLetter(c) && !char.IsNumber(c))
                {
                    if (buffer.Length > 0)
                    {
                        yield return buffer.ToString();
                        buffer.Clear();
                    }
                }
                else
                {
                    buffer.Append(c);
                }
            }
        }
    }
}
