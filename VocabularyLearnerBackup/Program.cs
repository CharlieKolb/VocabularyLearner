using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Input;

namespace VocabularyLearner
{
    class Program
    {

        


        static void Main(string[] args)
        {
            Program program = new Program();
            program.doTheThing();
            
        }

        private void doTheThing()
        {

            
            


            Application.Run(form);


        }

        

        
        private void DrawString()
        {
            System.Drawing.Graphics formGraphics = form.CreateGraphics();
            string drawString = "Sample Text";
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            float x = 150.0f;
            float y = 50.0f;
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y);
            drawFont.Dispose();
            drawBrush.Dispose();
            formGraphics.Dispose();
        }
    }

   
}
