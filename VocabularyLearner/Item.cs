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
        private List<string> validLanguageOne;
        private List<string> validLanguageTwo;

        private static Random random = new Random();

        //Creates item from the custom format &wordone&wordtwo<>&wordInOtherLanguageone&WordInOtherLanguageone
        public Item(String x) {
            validLanguageOne = new List<string>();
            validLanguageTwo = new List<string>();
            bool weRightNow = false;
            Regex validItem = new Regex("&[A-Za-z0-9가-힣 ]+");
            String nextValue = "";
            for (int i = 0; i < x.Length; i++)
            {
                if (x.ElementAt(i) == '\n')
                {
                    if (nextValue == "") break;
                    if (!weRightNow) validLanguageOne.Add(nextValue);
                    else validLanguageTwo.Add(nextValue);
                    break;
                }
                if (x.ElementAt(i) == '<' && x.ElementAt(i + 1) == '>')
                {
                    validLanguageOne.Add(nextValue);
                    nextValue = "";
                    weRightNow = true;
                    i++;
                    continue;
                }
                if (x.ElementAt(i) == '&')
                {
                    if (nextValue == "") continue;
                    if (!weRightNow) validLanguageOne.Add(nextValue);
                    else validLanguageTwo.Add(nextValue);
                    nextValue = "";
                    continue;
                }
                nextValue += x.ElementAt(i);
                if (i == x.Length - 1) {
                    if (!weRightNow) validLanguageOne.Add(nextValue);
                    else validLanguageTwo.Add(nextValue);
                }
            }
        }

        public string stringOfallResults(bool languageOne) {
            string output = "";
            List<string> list;
            if (languageOne) list = validLanguageTwo;
            else list = validLanguageOne;
            foreach (string x in list) {
                output += x + " | ";
            }

            return output.Substring(0, output.Length - 2);

        }

        public bool isCorrect(String input,bool ignoreCasing,bool languageOne) {
            List<string> list;
            if (languageOne) list = validLanguageTwo;
            else list = validLanguageOne;

            //
            foreach (string x in list)
            {
                if ((ignoreCasing && x.ToLower().Equals(input.ToLower()))) return true; 
                else if (x.Equals(input)) return true;
            }
            return list.Contains(input);
        }

        public String getToBeTranslated(bool languageOne) {
            if (languageOne) return validLanguageOne.ElementAt(random.Next(validLanguageOne.Count));
            else return validLanguageTwo.ElementAt(random.Next(validLanguageTwo.Count));
        }
        

        public void addWord(bool languageOne, string word) {
            if (languageOne) validLanguageOne.Add(word);
            else validLanguageTwo.Add(word);
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
