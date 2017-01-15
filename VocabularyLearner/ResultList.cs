using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabularyLearner
{
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

        public Item getNext() {
            switch (mode)
            {
                case mode_.AtoZ:
                    if (current == this.Count) current = 0;
                    return this.ElementAt(current++);
                case mode_.completeRandom:
                    int n = 0;
                    while ((n = random.Next(0, Count - 1)) == lastUsed) continue;   //Find a position != lastUsed
                    lastUsed = n;
                    return this.ElementAt(n);
                case mode_.randomIterate:
                    int k = 0;
                    while (usedThisCycle[this.ElementAt(k = random.Next(0, Count - 1))]) continue;   //Find a position != lastUsed
                    usedThisCycle[this.ElementAt(k = random.Next(0, Count - 1))] = true;
                    return this.ElementAt(k);
            }
            return null;
        }

        public void addItem(Item item) {
            this.Add(item);
            usedThisCycle[item] = true;
            
        }

        public static void setMode(mode_ mode) {
            ResultList.mode = mode;
        }

    }
}
