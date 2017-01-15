using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace VocabularyLearner
{
    class Drawable
    {
        public static readonly int result = 10;
        public static readonly int headline = 11;
        public static readonly int toBeTranslated = 12;
        public static readonly int solution = 13;
        public static readonly int textOne = 14;
        public static readonly int textTwo = 15;

        public string text;
        public  Point location;

        private bool draw = false;

        public Drawable(string text, Point location)
        {
            this.text = text;
            this.location = location;
            draw = true;
        }

        public Drawable(Point point) {
            text = "this should never be read";
            location = point;
            draw = false;
        }

        public bool doDraw() {
            return draw;
        }

        public void setDraw(bool input) {
            draw = input;
        }

        public void setText(string input) {
            text = input;
        }

        public string getText() {
            return text;
        }

        public Point getLocation() {
            return location;
        }
    }
}
