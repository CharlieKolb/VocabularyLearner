using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VocabularyLearner
{
    class Delay
    {

        private bool acceptInput = true;

        public void delayInput(Object millis) {
            try { int i = (int)millis; } catch (Exception) { return; }
            acceptInput = false;
            Thread.Sleep((int) millis);
            acceptInput = true;
        }

        public Boolean go() {
            return acceptInput;
        }
    }
}
