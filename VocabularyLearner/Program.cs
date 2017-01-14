using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace VocabularyLearner
{
    class Program : Control
    {
        private enum mode {
            AtoB,
            BtoA,
            RtoR
        }

        private MainFrame mainFrame;

        private LinkedList<Item> items;
        

        static void Main(string[] args)
        {
            Program program = new Program();
            
            program.doTheThing();
            
        }

        private void doTheThing()
        {
            items = (new FileHandler()).getItemList();
            Debug.WriteLine(items.Count());
            foreach (Item item in items) {
                Debug.WriteLine(item.ToString());
            }

            mainFrame = new MainFrame(this);
            Debug.WriteLine("AAAAA");
            mainFrame.DrawString("Hello", new Point(200, 300));
            mainFrame.Update();
            Application.Run(mainFrame.form);

        }
    }

   
}
