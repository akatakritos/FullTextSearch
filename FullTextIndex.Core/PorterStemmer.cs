using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FullTextIndex.Core
{
    /// <summary>
    /// http://snowball.tartarus.org/algorithms/porter/stemmer.html
    /// </summary>
    public class PorterStemmer
    {
        public string Stem(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var word = new Word(input);

            Step1A(word);
            Step1B(word);

            return word.Value;
        }

        /*
         * Step 1a
         * SSES    ->    SS    caresses    ->    caress
         * IES     ->    I     ponies      ->    poni
                               ties        ->    ti
         * SS      ->    SS    caress      ->    caress
         * S       ->          cats  ->  cat
         */
        private void Step1A(Word word)
        {
            if (word.EndsWith("sses"))
                 word.ReplaceSuffix("sses", "ss");
            else if (word.EndsWith("ies"))
                word.ReplaceSuffix("ies", "i");
            else if (word.EndsWith("ss"))
                word.ReplaceSuffix("ss", "ss");
            else if (word.EndsWith("s"))
                word.ReplaceSuffix("s", "");
        }

        private void Step1B(Word word)
        {
            if (word.EndsWith("eed"))
            {
                // prevent "feed" from falling into second rule and coming out "fe"
                // application of rule that only longest matching suffix applies
                if (word.ContainsVowelPreceding("eed"))
                    word.ReplaceSuffix("eed", "ee");
            }
            else if (word.ContainsVowelPreceding("ed"))
                word.ReplaceSuffix("ed", "");
            else if (word.ContainsVowelPreceding("ing"))
                word.ReplaceSuffix("ing", "");
        }

       

    }


}
