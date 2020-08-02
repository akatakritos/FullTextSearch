using System;
using System.Diagnostics;
using System.Xml.Schema;

namespace FullTextIndex.Core
{
    internal class Word
    {
        public string Value { get; private set; }
        public Word(string value)
        {
            Value = value;
        }

        public bool EndsWith(string suffix)
        {
            return Value.EndsWith(suffix);
        }

        public void ReplaceSuffix(string suffix, string replacement)
        {
            Value = Value.Substring(0, Value.Length - suffix.Length) + replacement;
        }

        /*
         * A consonant will be denoted by c, a vowel by v. A list ccc... of length greater
         * than 0 will be denoted by C, and a list vvv... of length greater than 0 will
         * be denoted by V. Any word, or part of a word, therefore has one of the four 
         * forms:
         *
         *     CVCV ... C
         *     CVCV ... V
         *     VCVC ... C
         *     VCVC ... V
         *
         * These may all be represented by the single form
         *
         *    [C]VCVC ... [V]
         *
         * where the square brackets denote arbitrary presence of their contents.
         * Using (VC)^m to denote VC repeated m times, this may again be written as
         *
         *   [C](VC)^m[V].
         *
         * m will be called the measure of any word or word part when represented in 
         * this form. The case m = 0 covers the null word. Here are some examples:
         *
         *   m=0        TR,   EE,   TREE,   Y,   BY.
         *   m=1        TROUBLE,   OATS,   TREES,   IVY.
         *   m=2        TROUBLES,   PRIVATE,   OATEN,   ORRERY.
         */
        public int Measure => MeasurePreceding("");

        public int MeasurePreceding(string suffix)
        {
            int m = 0;

            // "null word"
            if (Length == 0)
                return 0;

            // count transitions from V -> C
            // skipping first letter since we're looking for a transition
            for (int i = 1; i < Length - suffix.Length; i++)
            {
                if (IsConsonant(i) && IsVowel(i - 1))
                    m++;
            }

            return m;
        }

        public bool ContainsVowelPreceding(string suffix)
        {
            if (!EndsWith(suffix))
                return false;

            for (int i = 0; i < Length - suffix.Length; i++)
            {
                if (IsVowel(i))
                    return true;
            }

            return false;
        }


        public int Length => Value.Length;

        public char this[int index] => Value[index];

        /*
        * A consonant in a word is a letter other than A, E, I, O or U, and
        * other than Y preceded by a consonant.
        * 
        * (The fact that the term ‘consonant’ is defined to some extent in
        * terms of itself does not make it ambiguous.) So in TOY the consonants
        * are T and Y, and in SYZYGY they are S, Z and G. 
        * 
        * If a letter is not a consonant it is a vowel.
        */

        public bool IsConsonant(int index)
        {

            if (index < 0 || index >= Value.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            var c = Value[index];
            return c != 'a' && c != 'e' && c != 'i' && c != 'o' && c != 'u' && !IsYPrecededByConsonent(index);
        }

        private bool IsYPrecededByConsonent(int index)
        {
            Debug.Assert(index >= 0 && index < Value.Length);

            var c = Value[index];
            return c == 'y' && (index == 0 ? false : IsConsonant(index - 1));
        }

        public bool IsVowel(int index)
        {
            return !IsConsonant(index);
        }

        public bool EndsWithDoubleConsonant
        {
            get
            {
                if (Length < 2)
                    return false;

                return IsConsonant(Length - 1) && IsConsonant(Length - 2);
            }
        }

        public void CondenseDoubleSuffix()
        {
            Value = Value.Substring(0, Value.Length - 1);
        }

        public void Append(string suffix)
        {
            Value += suffix;
        }

        // the stem ends cvc, where the second c is not W, X or Y(e.g. -WIL, -HOP).
        public bool StarO()
        {
            if (Value.Length < 3) return false;
            var endsCVC = IsConsonant(Length - 3) && IsVowel(Length - 2) && IsConsonant(Length - 1);
            var secondC = this[Length - 1];

            return endsCVC && !(secondC == 'w' || secondC == 'x' || secondC == 'y');

        }
    }
}
