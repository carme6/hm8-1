using System.Configuration;
using System.Windows.Forms;

namespace hm8_1
{
    public partial class Form1 : Form
    {
         Bitmap b,b2,b3;
         Graphics g,g2,g3;
         Rectangle r1;
         Pen Pen;

        int pointnum = 10000;

        double minX=-100d;
        double maxX=100d;
        double minY=-100d;
        double maxY=100d;

        Random r_module;
        Random r_angle;

        int radius=100;

        List<Point> points;

        Dictionary<int, int> distr_x = new Dictionary<int, int>();
        Dictionary<int, int> distr_y = new Dictionary<int, int>();

        public Form1()
        {
            InitializeComponent();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);

            b2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g2 = Graphics.FromImage(b2);

            b3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            g3 = Graphics.FromImage(b3);

            r1 = new Rectangle(20, 20 , b.Width -40, b.Height -40);
            g.DrawRectangle(Pens.Black,r1);

            Pen = new Pen(Color.Black, 1);
        }

        

        

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            g2.Clear(Color.White);
            g3.Clear(Color.White);

            points = new List<Point>();

            r_module = new Random();
            r_angle = new Random();
     

            for (int i = 0; i < pointnum; i++)
            {
                double p_rand = r_module.NextDouble() * radius;
                double p_angle = r_angle.NextDouble() * 2 * Math.PI;
                double X = p_rand * Math.Cos(p_angle);
                double Y = p_rand * Math.Sin(p_angle);

                int x = fromXRealToXVirtual(X, minX, maxX, r1.Left, r1.Width);
                int y = fromYRealToYVirtual(Y , minY, maxY, r1.Top, r1.Height);

                Point p = new Point(x,y);
                points.Add(p);

                if (distr_x.ContainsKey(p.X))
                    distr_x[p.X]++;
                else
                    distr_x.Add(p.X, 1);

                if (distr_y.ContainsKey(p.Y))
                    distr_y[p.Y]++;
                else
                    distr_y.Add(p.Y, 1);
            }

            foreach (Point p in points)
            {
                Rectangle rect = new Rectangle(p.X - 1, p.Y - 1, 2, 2);
                g.FillEllipse(Brushes.Black, rect);
            }

            Rectangle hor_Histo = new Rectangle(20, 20, this.b2.Width - 40, this.b2.Height - 40);
            g2.DrawRectangle(Pens.Black, hor_Histo);
            createIstogramHoriz(hor_Histo, g2, 20, this.b2.Width - 40, distr_y);


            Rectangle vert_Histo = new Rectangle(20, 20, this.b3.Width - 40, this.b3.Height - 40);
            g3.DrawRectangle(Pens.Black, vert_Histo);
            createIstogramVert(vert_Histo, g3, 20 + this.b3.Height - 40, this.b3.Height - 40, distr_x);

            pictureBox1.Image = b;
            pictureBox2.Image = b2;
            pictureBox3.Image = b3;
        }

        public void createIstogramHoriz(Rectangle istogramSpace, Graphics g, int x, int w, Dictionary<int, int> distances)
        {
            int max_value = 0;
            foreach (int key in distances.Keys)
            {
                if (distances[key] > max_value)
                    max_value = distances[key];
            }

            Pen istoPen = new Pen(Color.Blue, 2);

            foreach (int key in distances.Keys)
            {
                double currentInterValue = distances[key];
                double pct = (double)distances[key] / (double)max_value;
                g.DrawLine(istoPen,
                           new PointF(x, key),
                           new PointF(x + ((int)(pct * w)), key)
                );
            }

        }

        public void createIstogramVert(Rectangle istogramSpace, Graphics g, int y, int w, Dictionary<int, int> distances)
        {
            int max_value = 0;
            foreach (int key in distances.Keys)
            {
                if (distances[key] > max_value)
                    max_value = distances[key];
            }

            Pen istoPen = new Pen(Color.Blue, 2);

            foreach (int key in distances.Keys)
            {
                double currentInterValue = distances[key];
                double pct = (double)distances[key] / (double)max_value;
                g.DrawLine(istoPen,
                           new PointF(key, y - ((int)(pct * w))),
                           new PointF(key, y)
                );
            }

        }


        private int fromXRealToXVirtual(double x, double minX, double maxX, int left, int w)
        {
            return left + (int)(w * (x - minX) / (maxX - minX));
        }

        private int fromYRealToYVirtual(double y, double minY, double maxY, int top, int h)
        {
            return top + (int)(h - h * (y - minY) / (maxY - minY));
        }

    
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pointnum=trackBar1.Value;
            label1.Text = "Number of points : " + pointnum.ToString();
        }
    }
}