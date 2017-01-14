using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VocabularyLearner
{
    class FileHandler
    {
        

        private string path = Directory.GetCurrentDirectory();

        private LinkedList<string> getLineList() {
            string line;
            LinkedList<string> output = new LinkedList<string>();
            // Read the file and display it line by line. 
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@"wordbook.txt");
                while ((line = file.ReadLine()) != null)
                {
                    output.AddLast(line);
                }

                file.Close();
            }
            catch (FileNotFoundException) //create file if it doesn't exist
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"wordbook.txt");
                file.WriteLine("#Format: &varient1Language1&variantXLanguage1<>&varient1Language2&varientXLanguage2<NEWLINE(Enter)>");
                file.WriteLine("#Lines not it that format will be ignored");
                file.WriteLine("#Lines not it that format will be marked with '>'");
                file.WriteLine("#Lines started with # will be ignored");


                file.Close();
            }

            return output;
        }

        public LinkedList<Item> getItemList() {
            LinkedList<string> input = getLineList();


            LinkedList<Item> output = new LinkedList<Item>();
            string[] arrLines = input.ToArray<string>();
            bool changedLine = false;
            //Translate input to Items
            Regex validLine = new Regex("(&[A-Za-z0-9가-힣 ]+)+<>(&[A-Za-z0-9가-힣 ]+)+");
            for(int i = 0; i < arrLines.Length; i++) {
                String x = arrLines[i];

                if (x.StartsWith("#") || x.StartsWith(">")|| x.StartsWith("DELETED")) continue;
                if (validLine.IsMatch(x))
                {
                    output.AddLast(new Item(x));
                }
                else
                {
                    arrLines[i] = ">" + arrLines[i];
                    changedLine = true;
                }
            }

            if(changedLine) File.WriteAllLines(@"wordbook.txt", arrLines);


            return output;
        }

        public bool removeItem(Item item) {
            LinkedList<string> input = getLineList();


            LinkedList<Item> output = new LinkedList<Item>();
            string[] arrLines = input.ToArray<string>();
            bool changedLine = false;
            //Translate input to Items
            Regex validLine = new Regex("(&[A-Za-z0-9가-힣 ]+)+<>(&[A-Za-z0-9가-힣 ]+)+");
            for (int i = 0; i < arrLines.Length; i++)
            {
                String x = arrLines[i];

                if (x.StartsWith("#") || x.StartsWith(">")) continue;
                if (x.Equals(item.ToString()))
                {
                    arrLines[i] = "DELETED" +arrLines[i];
                    changedLine = true;
                }

                
            }
            if (changedLine) File.WriteAllLines(@"wordbook.txt", arrLines);
            return changedLine;
        }

        private Item createItem(string x) {

            return new Item(x);
        }
    }
}
