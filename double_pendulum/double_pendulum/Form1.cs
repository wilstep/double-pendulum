using System;
using System.Drawing;
using System.Windows.Forms;

namespace double_pendulum
{
    public partial class Pendulum : Form
    {
        int width, height;
        int hWidth, hHeight;
        double rad, theta1, theta2;
        // bools to keep track of what the program is doing
        bool run, first, first_start, message, win_min;
        Bitmap bufl;
        Motion p_mot;

        public Pendulum()
        {
            InitializeComponent();
            width = pictureBox1.Width;
            height = pictureBox1.Height;
            hWidth = width / 2;
            hHeight = height / 2;
            rad = 0.46 * hWidth;
            bufl = new Bitmap(width, height);
            run = false;
            first = true;
            first_start = true;
            message = true;
            win_min = false;
            theta1 = theta2 = 0.0;
            p_mot = new Motion(1.0, 1.0, 1.0); // unit mass ratio, both lengths set to unity
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (message) draw_message();
            else draw_pendulum();
        }

        private void message_button_Click(object sender, EventArgs e)
        {
            message = true;
            run = false;
            draw_message();
        }

        private void set_button_Click(object sender, EventArgs e)
        {
            double theta0;

            theta0 = (double)numericUpDown1.Value;
            theta0 *= Math.PI / 180.0;
            p_mot.Reset(theta0, theta0);
            theta1 = p_mot.GetTh1();
            theta2 = p_mot.GetTh2();
            draw_pendulum();
            run = false;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            double theta0;

            if (theta1 == 0.0 && theta2 == 0.0) first_start = true;
            if (first_start)
            {
                theta0 = (double)numericUpDown1.Value;
                theta0 *= Math.PI / 180.0;
                p_mot.Reset(theta0, theta0);
                theta1 = p_mot.GetTh1();
                theta2 = p_mot.GetTh2();
                draw_pendulum();
                first_start = false;
            }
            run = true;
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            run = false;
        }

        // method to draw pendulum
        private void draw_pendulum()
        {
            int x1, y1, x2, y2;

            message = false;
            // convert pendulum positions to screen coordinates
            x1 = Convert.ToInt32(rad * Math.Sin(theta1) + hWidth);
            y1 = Convert.ToInt32(rad * Math.Cos(theta1) + hHeight);
            x2 = x1 + Convert.ToInt32(rad * Math.Sin(theta2));
            y2 = y1 + Convert.ToInt32(rad * Math.Cos(theta2));
            SolidBrush myBrush;
            Pen myPen;
            myBrush = new SolidBrush(Color.Blue);
            myPen = new Pen(Color.Blue, 5);
            // rectangles to draw circles used in drawing pendulum
            Rectangle rect0, rect1, rect2;
            rect0 = new Rectangle(hWidth-9, hHeight-9, 18, 18);
            rect1 = new Rectangle(x1-18, y1-18, 36, 36);
            rect2 = new Rectangle(x2 - 18, y2 - 18, 36, 36);
            // paint background white, then draw pendulum
            using (Graphics g = Graphics.FromImage(bufl))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
                g.FillEllipse(myBrush, rect0);
                g.FillEllipse(myBrush, rect1);
                g.FillEllipse(myBrush, rect2);
                g.DrawLine(myPen, hWidth, hHeight, x1, y1);
                g.DrawLine(myPen, x1, y1, x2, y2);
                pictureBox1.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
            }
        }

        // method to display message instead of drawing pendulum
        private void draw_message()
        {
            using (Graphics g = Graphics.FromImage(bufl))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, width, height));
                // Create string to draw.
                String drawString = "This program simulates a double jointed pendulum," +
                    "\nwhich forms a nice example of Chaos in a dynamical" +
                    "\nsystem. At small initial angles the motion will look" +
                    "\nregular, but at larger initial angles the motion" +
                    "\nwill become very chaotic. Start with say 70 degrees" +
                    "\nand then observe what happens as you steadily" +
                    "\nincrease the initial angle to and above 80 degrees." +
                    "\n\nIf you make the initial angle too small there will" +
                    "\nbe very little to see";

                // Create font and brush.
                Font drawFont = new Font("Arial", 18);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                // Create point for upper-left corner of drawing.
                PointF drawPoint = new PointF(150.0F, 150.0F);
                // Draw string to screen.
                g.DrawString(drawString, drawFont, drawBrush, drawPoint);
                pictureBox1.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // keep track of when the form has been brought back after minimising window
            if (WindowState == FormWindowState.Minimized) win_min = true; 
            if (WindowState != FormWindowState.Minimized && win_min)
            {
                // redraw after minimising window
                if (message) draw_message(); 
                else draw_pendulum();           
                win_min = false;
            }
            if (first)
            {
                draw_message();
                first = false;
                return;
            }
            if (run)
            {
                draw_pendulum();
                p_mot.Run(40);
                theta1 = p_mot.GetTh1();
                theta2 = p_mot.GetTh2();
                return;
            }
        }
    }
}
