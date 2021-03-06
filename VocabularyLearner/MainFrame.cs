﻿using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

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

        private Delay enterDelay = new Delay();

        private bool AtoB = true;
        private bool ignoreCasing = false;

        private mode_ mode = mode_.AtoB;    //Directs the translation direction, A->B, B->A or random

        private state_ state = state_.waitingForSolution;

        private TextBox inputBox = new TextBox();
        private ComboBox directionBox = new ComboBox();
        private ComboBox shuffleBox = new ComboBox();
        private CheckBox caseButton = new CheckBox();

        private Graphics formGraphics;

        private Font font;

        private Brush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

        private Drawable[] drawables = new Drawable[20];

        private Random random = new Random();

        public MainFrame()
        {
            SetBounds(300, 400, 1200, 600);
            formGraphics = CreateGraphics();

            PrivateFontCollection modernFont = new PrivateFontCollection();
            modernFont.AddFontFile("NanumGothic.ttf");

            font = new Font(modernFont.Families[0], 18);
            
            //Adds the TextBox for the User Input
            inputBox.SetBounds(230, 200, 300, 30);
            Controls.Add(inputBox);
            inputBox.KeyPress += Box_KeyPress;
            inputBox.Font = font;
            BringToFront();

            //Loads the items read into the program by the FileHandler
            items = FileHandler.getItemList();
            current = items.getNext();

            //Initialize the different drawing subjects
            drawables[Drawable.headline] = new Drawable("Translator-Test Press Enter to proceed", new Point(10, 10));
            drawables[Drawable.textOne] = new Drawable("Please translate: ", new Point(5, 170));
            drawables[Drawable.toBeTranslated] = new Drawable(current.getToBeTranslated(AtoB), new Point(227, 170));
            drawables[Drawable.solution] = new Drawable(current.stringOfallResults(AtoB), new Point(227, 240));
            drawables[Drawable.textTwo] = new Drawable("Possible solutions:", new Point(5, 240));
            drawables[Drawable.solution].setDraw(false);
            drawables[Drawable.textTwo].setDraw(false);
            drawables[Drawable.result] = new Drawable(new Point(230, 300));


            //Dropdown menu to select the direction of the questions
            directionBox.SetBounds(600, 100, 100, 10);
            directionBox.DropDownStyle = ComboBoxStyle.DropDownList;
            directionBox.Name = "Direction";
            directionBox.DataSource = new string[]{"A -> B", "B -> A", "Random"};
            directionBox.DropDownClosed += DirectionBox_DropDownClosed;

            //Dropdown menu to select the order of the questions
            shuffleBox.SetBounds(600, 200, 100, 10);
            shuffleBox.DropDownStyle = ComboBoxStyle.DropDownList;
            shuffleBox.Name = "Shuffle";
            shuffleBox.DataSource = new string[] { "In order", "Truly random", "Random A-Z"};
            shuffleBox.DropDownClosed += ShuffleBox_DropDownClosed;

            caseButton.SetBounds(600, 300, 200, 30);
            caseButton.Text = "Ignore upper/lower case";
            caseButton.CheckedChanged += CaseButton_CheckedChanged;

            Controls.Add(directionBox);
            Controls.Add(shuffleBox);
            Controls.Add(caseButton);

            Application.Run(this);
        }

        private void CaseButton_CheckedChanged(object sender, EventArgs e)
        {
            ignoreCasing = caseButton.Checked;
        }

        private void ShuffleBox_DropDownClosed(object sender, EventArgs e)
        {
            if (shuffleBox.SelectedIndex == 0) ResultList.setMode(ResultList.mode_.AtoZ);//In order
            else if (shuffleBox.SelectedIndex == 1) ResultList.setMode(ResultList.mode_.completeRandom);//random
            else if (shuffleBox.SelectedIndex == 2) ResultList.setMode(ResultList.mode_.randomIterate);//random A-Z
        }

        private void DirectionBox_DropDownClosed(object sender, EventArgs e)
        {
            if (directionBox.SelectedIndex == 0) mode = mode_.AtoB;//A->B
            else if (directionBox.SelectedIndex == 1) mode = mode_.BtoA;//A->B
            else if (directionBox.SelectedIndex == 2) mode = mode_.RtoR;//A->B
           
        }

        private Item getRandomItem() {
            return items.ElementAt(random.Next(0, items.Count - 1));
        }
        

        private void Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && enterDelay.go()) //Enter-Key und possible Delay
            {
                switch (state) {
                    //User has put in a solution -> Result
                    case state_.waitingForSolution:
                        bool isCorrect = current.isCorrect(inputBox.Text, ignoreCasing, AtoB);
                        if (isCorrect) drawables[Drawable.result].setText("Correct!");
                        else drawables[Drawable.result].setText("Wrong!");
                        drawables[Drawable.result].setDraw(true);
                        drawables[Drawable.solution].setDraw(true);
                        drawables[Drawable.textTwo].setDraw(true);
                        state = state_.waitingForContinue;
                        (new Thread(enterDelay.delayInput)).Start(400); //Sets enterDelay.acceptInput to false for 400 millis to protect the user from revealing the solution immidiately

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
