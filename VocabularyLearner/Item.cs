using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VocabularyLearner
{
    class Item
    {
        private LinkedList<string> validLanguageOne;
        private LinkedList<string> validLanguageTwo;

        //Creates item from the custom format &wordone&wordtwo<>&wordInOtherLanguage&WordInOtherLanguage
        public Item(String x) {
            validLanguageOne = new LinkedList<string>();
            validLanguageTwo = new LinkedList<string>();
            bool weRightNow = false;
            Regex validItem = new Regex("&[A-Za-z0-9가-힣 ]+");
            String nextValue = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x.ElementAt(i) == '\n')
                {
                    if (nextValue == "") break;
                    if (!weRightNow) validLanguageOne.AddLast(nextValue);
                    else validLanguageTwo.AddLast(nextValue);
                    break;
                }
                if (x.ElementAt(i) == '<' && x.ElementAt(i + 1) == '>')
                {
                    validLanguageOne.AddLast(nextValue);
                    nextValue = "";
                    weRightNow = true;
                    i++;
                    continue;
                }
                if (x.ElementAt(i) == '&')
                {
                    if (nextValue == "") continue;
                    if (!weRightNow) validLanguageOne.AddLast(nextValue);
                    else validLanguageTwo.AddLast(nextValue);
                    nextValue = "";
                    continue;
                }
                nextValue += x.ElementAt(i);
                if (i == x.Length - 1) {
                    if (!weRightNow) validLanguageOne.AddLast(nextValue);
                    else validLanguageTwo.AddLast(nextValue);
                }
            }
        }
        

        public void addWord(bool languageOne, string word) {
            if (languageOne) validLanguageOne.AddFirst(word);
            else validLanguageTwo.AddLast(word);
        }

        override
        public String ToString() {
            String output = "";
            foreach (string x in validLanguageOne) output += "&" + x;
            output += "<>";
            foreach (string x in validLanguageTwo) output += "&" + x;
            return output;
        }
    }
}
