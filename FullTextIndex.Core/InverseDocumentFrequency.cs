using System;
using System.Collections.Generic;
using System.Text;

namespace FullTextIndex.Core
{
    public static class InverseDocumentFrequency
    {
        public static float For(int documentsWithTerm, int documentsCount)
        {
            var x = (documentsCount - documentsWithTerm + 0.5) / (documentsWithTerm + 0.5);
            return (float)Math.Log(1 + Math.Abs(x));
        }
    }
}
