using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabularyLearner
{
    class Drawable
    {
        public readonly string text;
        public readonly Point location;

        public Drawable(string text, Point location) {
            this.text = text;
            this.location = location;
        }
    }
}
