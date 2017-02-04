using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabularyLearner
{
    //A list implementation that additionally has a dictionary for its items for the random A-Z option
    class ResultList : List<Item>   
    {
        public enum mode_ {
            AtoZ,
            completeRandom,
            randomIterate
        }

        private static mode_ mode;

        private int current;

        private Random random;

        private int lastUsed; //Used for completeRandom cycle, so no element is returned twice in a row

        private Dictionary<Item, bool> usedThisCycle; //Used for randomIterate cycle, to mark which elements have already been used

        public ResultList() {
            usedThisCycle = new Dictionary<Item, bool>();
            current = 0;
            mode = mode_.AtoZ;
            random = new Random();
        }

        //Returns the next item according to the current mode
        public Item getNext() {
            switch (mode)
            {
                case mode_.AtoZ:
                    if (current == this.Count) current = 0;
                    return this.ElementAt(current++);
                case mode_.completeRandom:
                    int n = 0;
                    while ((n = random.Next(0, Count)) == lastUsed) continue;   //Find a position != lastUsed
                    lastUsed = n;
                    return this.ElementAt(n);
                case mode_.randomIterate:
                    Item k = null;
                    if (!usedThisCycle.ContainsValue(false)) resetDictionary();
                    while (usedThisCycle[k = usedThisCycle.ElementAt(random.Next(0, Count)).Key]) continue;   //Find an unused
                    usedThisCycle[k] = true;
                    return k;
            }
            return null;
        }

        private void resetDictionary() {
            for (int i = 0; i < usedThisCycle.Count; i++) {
                usedThisCycle[usedThisCycle.ElementAt(i).Key] = false;
            }
        }

        public void addItem(Item item) {
            Add(item);
            usedThisCycle.Add(item, false);
            
        }

        public void clearItems() {
            Clear(); //Deleted items in the List itself
            usedThisCycle.Clear(); //Deletes items in the lookup-directory used in the mode_.randomIterate mode
        }

        public static void setMode(mode_ mode) {
            ResultList.mode = mode;
        }

    }
}
