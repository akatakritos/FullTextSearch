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
            Step1C(word);
            Step2(word);
            Step3(word);

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

        /*
         * Step 1b
         *   (m>0) EED    ->    EE     feed        ->     feed
         *                             agreed      ->     agree
         *    (*v*) ED    ->           plastered   ->     plaster
         *                             bled        ->     bled
         *    (*v*) ING   ->           motoring    ->     motor
         *                             sing        ->     sing
         */
        private void Step1B(Word word)
        {
            bool postProcessingRequired = false;

            if (word.EndsWith("eed"))
            {
                // prevent "feed" from falling into second rule and coming out "fe"
                // application of rule that only longest matching suffix applies
                if (word.ContainsVowelPreceding("eed"))
                    word.ReplaceSuffix("eed", "ee");
            }
            else if (word.ContainsVowelPreceding("ed"))
            {
                word.ReplaceSuffix("ed", "");
                postProcessingRequired = true;
            }
            else if (word.ContainsVowelPreceding("ing"))
            {
                word.ReplaceSuffix("ing", "");
                postProcessingRequired = true;
            }


            /*
             * If the second or third of the rules in Step 1b is successful,
             * the following is done:
             * 
             *    AT                                   ->    ATE                conflat(ed)        ->        conflate
             *    BL                                   ->    BLE                troubl(ed)        ->        trouble
             *    IZ                                   ->    IZE                siz(ed)        ->        size
             *    (*d and not (*L or *S or *Z))        ->    single letter      hopp(ing)        ->        hop
             *                                                                  tann(ed)        ->        tan
             *                                                                  fall(ing)        ->        fall
             *                                                                  hiss(ing)        ->        hiss
             *                                                                  fizz(ed)        ->        fizz
             *   (m=1 and *o)                          ->    E                  fail(ing)        ->        fail
             *                                                                  fil(ing)        ->        file
             *
             *  The rule to map to a single letter causes the removal of one
             *  of the double letter pair. The -E is put back on -AT, -BL and -IZ, 
             *  so that the suffixes -ATE, -BLE and -IZE can be recognised later. 
             *  This E may be removed in step 4.
             */

            if (postProcessingRequired)
            {
                if (word.EndsWith("at"))
                    word.ReplaceSuffix("at", "ate");
                else if (word.EndsWith("bl"))
                    word.ReplaceSuffix("bl", "ble");
                else if (word.EndsWith("iz"))
                    word.ReplaceSuffix("iz", "ize");
                else if (word.EndsWithDoubleConsonant && !(word.EndsWith("l") || word.EndsWith("s") || word.EndsWith("z")))
                    word.CondenseDoubleSuffix();
                else if (word.Measure == 1 && word.StarO())
                    word.Append("e");
            }

        }

        /**
         * Step 1 C
         *
         *    (*v*) Y   ->   I    happy   ->     happi
                                  sky     ->     sky
        */
        private void Step1C(Word word)
        {
            if (word.ContainsVowelPreceding("y"))
                word.ReplaceSuffix("y", "i");
        }

        private void Step2(Word word)
        {
            if (word.MeasurePreceding("ational") > 0 && word.EndsWith("ational"))
                word.ReplaceSuffix("ational", "ate");
            else if (word.MeasurePreceding("tional") > 0 && word.EndsWith("tional"))
                word.ReplaceSuffix("tional", "tion");
            else if (word.MeasurePreceding("enci") > 0 && word.EndsWith("enci"))
                word.ReplaceSuffix("enci", "ence");
            else if (word.MeasurePreceding("anci") > 0 && word.EndsWith("anci"))
                word.ReplaceSuffix("anci", "ance");
            else if (word.MeasurePreceding("izer") > 0 && word.EndsWith("izer"))
                word.ReplaceSuffix("izer", "ize");
            else if (word.MeasurePreceding("abli") > 0 && word.EndsWith("abli"))
                word.ReplaceSuffix("abli", "able");
            else if (word.MeasurePreceding("alli") > 0 && word.EndsWith("alli"))
                word.ReplaceSuffix("alli", "al");
            else if (word.MeasurePreceding("entli") > 0 && word.EndsWith("entli"))
                word.ReplaceSuffix("entli", "ent");
            else if (word.MeasurePreceding("eli") > 0 && word.EndsWith("eli"))
                word.ReplaceSuffix("eli", "e");
            else if (word.MeasurePreceding("ousli") > 0 && word.EndsWith("ousli"))
                word.ReplaceSuffix("ousli", "ous");
            else if (word.MeasurePreceding("ization") > 0 && word.EndsWith("ization"))
                word.ReplaceSuffix("ization", "ize");
            else if (word.MeasurePreceding("ation") > 0 && word.EndsWith("ation"))
                word.ReplaceSuffix("ation", "ate");
            else if (word.MeasurePreceding("ator") > 0 && word.EndsWith("ator"))
                word.ReplaceSuffix("ator", "ate");
            else if (word.MeasurePreceding("alism") > 0 && word.EndsWith("alism"))
                word.ReplaceSuffix("alism", "al");
            else if (word.MeasurePreceding("iveness") > 0 && word.EndsWith("iveness"))
                word.ReplaceSuffix("iveness", "ive");
            else if (word.MeasurePreceding("fulness") > 0 && word.EndsWith("fulness"))
                word.ReplaceSuffix("fulness", "ful");
            else if (word.MeasurePreceding("ousness") > 0 && word.EndsWith("ousness"))
                word.ReplaceSuffix("ousness", "ous");
            else if (word.MeasurePreceding("aliti") > 0 && word.EndsWith("aliti"))
                word.ReplaceSuffix("aliti", "al");
            else if (word.MeasurePreceding("iviti") > 0 && word.EndsWith("iviti"))
                word.ReplaceSuffix("iviti", "ive");
            else if (word.MeasurePreceding("biliti") > 0 && word.EndsWith("biliti"))
                word.ReplaceSuffix("biliti", "ble");

            /*
             * TODO
             * The test for the string S1 can be made fast by doing a program switch on the 
             * penultimate letter of the word being tested. This gives a fairly even breakdown
             * of the possible values of the string S1. It will be seen in fact that the 
             * S1-strings in step 2 are presented here in the alphabetical order of their
             * penultimate letter. Similar techniques may be applied in the other steps.
             */
        }

        private void Step3(Word word)
        {
            if (word.MeasurePreceding("icate") > 0 && word.EndsWith("icate"))
                word.ReplaceSuffix("icate", "ic");
            else if (word.MeasurePreceding("ative") > 0 && word.EndsWith("ative"))
                word.ReplaceSuffix("ative", "");
            else if (word.MeasurePreceding("alize") > 0 && word.EndsWith("alize"))
                word.ReplaceSuffix("alize", "al");
            else if (word.MeasurePreceding("iciti") > 0 && word.EndsWith("iciti"))
                word.ReplaceSuffix("iciti", "ic");
            else if (word.MeasurePreceding("ical") > 0 && word.EndsWith("ical"))
                word.ReplaceSuffix("ical", "ic");
            else if (word.MeasurePreceding("ful") > 0 && word.EndsWith("ful"))
                word.ReplaceSuffix("ful", "");
            else if (word.MeasurePreceding("ness") > 0 && word.EndsWith("ness"))
                word.ReplaceSuffix("ness", "");

        }


    }

}
