using System;
using System.Drawing;
using System.Windows.Forms;

namespace double_pendulum
{
    public partial class Pendulum : Form
    {
        int width, height;
        int hWidth, hHeight;
        double rad, theta;
        bool run;
        Bitmap bufl;


        public Pendulum()
        {
            InitializeComponent();
            width = pictureBox1.Width;
            height = pictureBox1.Height;
            bufl = new Bitmap(width, height);
            run = false;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            run = true;
            Bitmap bufl = new Bitmap(width, height);
            hWidth = width / 2;
            hHeight = height / 2;
            rad = 0.92 * hWidth;
            draw();
        }

        private void draw()
        {
            double x1, y1;

            x1 = rad * Math.Sin(theta) + hWidth;
            y1 = rad * Math.Cos(theta) + hHeight;
            SolidBrush myBrush;
            myBrush = new SolidBrush(Color.Blue);
            Rectangle rect0, rect1;
            rect0 = new Rectangle(hWidth-9, hHeight-9, 18, 18);
            rect1 = new Rectangle(Convert.ToInt32(x1)-18, Convert.ToInt32(y1)-18, 36, 36);
            using (Graphics g = Graphics.FromImage(bufl))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
                //g.DrawEllipse(myPen, rect);
                g.FillEllipse(myBrush, rect0);
                g.FillEllipse(myBrush, rect1);
                pictureBox1.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
            }
        }

            /* if (run)
            {
                draw();
        theta += 0.01;
            }*/

    private void timer1_Tick(object sender, EventArgs e)
        {
            if (run)
            {
                draw();
                theta += 0.01;
            }
        }
    }
}
