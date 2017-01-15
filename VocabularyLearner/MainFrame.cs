using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace VocabularyLearner
{
    class MainFrame : Form
    {
        private enum mode_
        {
            AtoB,
            BtoA,
            RtoR
        } //States the current mode, either language A -> language B, reverse or random

        private enum state_ {
            waitingForSolution,
            waitingForContinue
        }

        private ResultList items;
        private Item current;


        static void Main(string[] args)
        {
            MainFrame mainFrame = new MainFrame();

        }

        private bool AtoB = true;

        private mode_ mode = mode_.AtoB;    //Directs the translation direction, A->B, B->A or random

        private state_ state = state_.waitingForSolution;

        private TextBox inputBox = new TextBox();
        private ComboBox shuffleBox = new ComboBox();


        private Graphics formGraphics;

        private Font font = new Font(new FontFamily("Arial"), 16, FontStyle.Regular, GraphicsUnit.Pixel);

        private Brush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

        private Drawable[] drawables = new Drawable[20];

        private Random random = new Random();

        public MainFrame()
        {
            SetBounds(300, 400, 1200, 600);
            formGraphics = CreateGraphics();
            
            //Adds the TextBox for the User Input
            inputBox.SetBounds(230, 200, 200, 30);
            Controls.Add(inputBox);
            inputBox.KeyPress += Box_KeyPress;
            BringToFront();

            //Loads the items read into the program by the FileHandler
            items = (new FileHandler()).getItemList();
            Debug.WriteLine(items.Count);
            foreach (Item item in items)
            {
                Debug.WriteLine(item.ToString());
            }
            current = items.getNext();

            //Initialize the different drawing subjects
            drawables[Drawable.headline] = new Drawable("Translator-Test Press Enter to proceed", new Point(10, 10));
            drawables[Drawable.textOne] = new Drawable("Please translate: ", new Point(70, 170));
            drawables[Drawable.toBeTranslated] = new Drawable(current.getToBeTranslated(AtoB), new Point(227, 170));
            drawables[Drawable.solution] = new Drawable(current.stringOfallResults(AtoB), new Point(227, 230));
            drawables[Drawable.textTwo] = new Drawable("Possible solutions:", new Point(70, 230));
            drawables[Drawable.solution].setDraw(false);
            drawables[Drawable.textTwo].setDraw(false);
            drawables[Drawable.result] = new Drawable(new Point(230, 300));


            //Dropdown menu to select the Direction of the questions
            shuffleBox.SetBounds(600, 100, 70, 10);
            shuffleBox.DropDownStyle = ComboBoxStyle.DropDownList;
            shuffleBox.Name = "Direction";
            shuffleBox.DataSource = new string[]{"A -> B", "B -> A", "Random"};
            shuffleBox.DropDownClosed += ShuffleBox_DropDownClosed;
            Controls.Add(shuffleBox);
           

            Application.Run(this);
        }

        private void ShuffleBox_DropDownClosed(object sender, EventArgs e)
        {
            if (shuffleBox.SelectedIndex == 0) mode = mode_.AtoB;//A->B

            else if (shuffleBox.SelectedIndex == 1) mode = mode_.BtoA;//A->B

            else if (shuffleBox.SelectedIndex == 2) mode = mode_.RtoR;//A->B
           
        }

        private Item getRandomItem() {
            return items.ElementAt(random.Next(0, items.Count - 1));
        }
        

        private void Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //Enter-Taste
            {
                switch (state) {
                    //User has put in a solution -> Result
                    case state_.waitingForSolution:
                        bool isCorrect = current.isCorrect(AtoB, inputBox.Text);
                        if (isCorrect) drawables[Drawable.result].setText("Correct!");
                        else drawables[Drawable.result].setText("Wrong!");
                        drawables[Drawable.result].setDraw(true);
                        drawables[Drawable.solution].setDraw(true);
                        drawables[Drawable.textTwo].setDraw(true);
                        state = state_.waitingForContinue;
                        break;
                    //User pushed continue -> Generate new word
                    case state_.waitingForContinue:
                        switch (mode) {     //Set bool AtoB according to selected mode
                            case mode_.AtoB: AtoB = true; break;
                            case mode_.BtoA: AtoB = false; break;
                            case mode_.RtoR: AtoB = random.Next(0, 2)==1 ? true : false; break; //Randomly true or false
                        }
                        current = items.getNext();
                        drawables[Drawable.result].setDraw(false);
                        drawables[Drawable.solution].setDraw(false);
                        drawables[Drawable.textTwo].setDraw(false);
                        drawables[Drawable.toBeTranslated].setText(current.getToBeTranslated(AtoB));
                        drawables[Drawable.solution].setText(current.stringOfallResults(AtoB));
                        inputBox.Clear();
                        state = state_.waitingForSolution;
                        break;
                }
                e.Handled = true;
                InvokePaint(this, new PaintEventArgs(formGraphics, DisplayRectangle));

            }
        }
        
        
        protected override void OnPaint(PaintEventArgs p)
        {
            formGraphics = CreateGraphics();
            formGraphics.Clear(BackColor);
            
            base.OnPaint(p);
            try
            {   
                if (drawables[Drawable.result].doDraw())            draw(drawables[Drawable.result], formGraphics);
                if (drawables[Drawable.headline].doDraw())          draw(drawables[Drawable.headline], formGraphics);
                if (drawables[Drawable.toBeTranslated].doDraw())    draw(drawables[Drawable.toBeTranslated], formGraphics);
                if (drawables[Drawable.solution].doDraw())          draw(drawables[Drawable.solution], formGraphics);
                if (drawables[Drawable.textOne].doDraw())           draw(drawables[Drawable.textOne], formGraphics);
                if (drawables[Drawable.textTwo].doDraw())           draw(drawables[Drawable.textTwo], formGraphics);


            }
            catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
            }
            
            formGraphics.Dispose();

        }

        private void draw(Drawable drawable, Graphics formGraphics) {
            formGraphics.DrawString(drawable.text, font, solidBrush, drawable.location);
        }


    }
}
