using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System;

namespace VocabularyLearner
{
    class MainFrame : Control
    {
        public readonly Form form = new Form();

        private TextBox inputBox = new TextBox();

        private Graphics formGraphics;

        private Font font = new Font(new FontFamily("Arial"), 20, FontStyle.Regular, GraphicsUnit.Pixel);

        private Brush solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

        private Queue<Drawable> toBeDrawn = new Queue<Drawable>();

        public MainFrame(Program program)
        {
            form.SetBounds(300, 400, 1200, 600);
            formGraphics = form.CreateGraphics();
            form.Controls.Add(program);
            
            inputBox.SetBounds(200, 200, 200, 30);
            form.Controls.Add(inputBox);
            inputBox.KeyPress += Box_KeyPress;
            form.BringToFront();
            form.Update();
            form.Visible = true;
            form.Invalidate();
        }
        
        private void Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //Enter-Taste
            {
                
                DrawString(inputBox.Text, new Point(350, 250));
                e.Handled = true;
                string text = Directory.GetCurrentDirectory();
                DrawString(text, new Point(0, 400));
                
            }
        }
        
        
        protected override void OnPaint(PaintEventArgs p) {
            Debug.WriteLine("In OnPaint");
            base.OnPaint(p);
            try
            {
                foreach(Drawable current in toBeDrawn) { 
                    TextRenderer.DrawText(formGraphics, current.text, font, current.location, Color.Black);
                }
            }
            catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
            }
        }

        



        public void DrawString(string text, Point point)
        {
            try
            { 
                toBeDrawn.Enqueue(new Drawable(text, point));
                Debug.WriteLine("Enqueued " + text);
            }
            catch (System.ArgumentException e)
            {
                Debug.WriteLine("Error when drawing string " + text + " | " + e.Message);
            }
            finally {
                InvokePaint(this, new PaintEventArgs(formGraphics, form.DisplayRectangle));
                Debug.WriteLine("In DrawString finally");
            }
        }

    }
}
