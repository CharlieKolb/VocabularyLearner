using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;

namespace VocabularyLearner
{
    class MainFrame
    {
        private Form form = new Form();

        private Label label = new Label();

        private TextBox inputBox = new TextBox();

        private Graphics formGraphics;

        public void initiate()
        {
            form.SetBounds(300, 400, 1200, 600);
            formGraphics = form.CreateGraphics();
        }

        
        public void drawString(string text, PointF pointf) 
        {
            var fontFamily = new FontFamily("Arial");
            var font = new Font(fontFamily, 32, FontStyle.Regular, GraphicsUnit.Pixel);
            var solidBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

            formGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            formGraphics.DrawString("Your Text Here", font, solidBrush, new PointF(10, 60));

            inputBox.Location = new Point(200, 200);
            form.Controls.Add(inputBox);
            inputBox.BringToFront();
            inputBox.KeyPress += Box_KeyPress;
        }

        private void Box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) //Enter-Taste
            {
                updateLabel(inputBox.Text);
                form.Update();
                e.Handled = true;
            }
        }

        private void updateLabel(string text)
        {

            label.Text = text;
            form.Controls.Add(label);
            label.BringToFront();

        }

    }
}
