namespace balls_and_rectangle
{
    public partial class Form1 : Form
    {
        bool flagButtonAdd;

        Random rnd = new Random();

        private List<Square> squares = new List<Square>();
        private List<Circle> circles = new List<Circle>();
        private Graphics g;
        private BufferedGraphics buff_g;
        Pen pen = new Pen(Color.Black);
        private Pen pen_g;
        Size ContainerSize;

        public Form1()
        {
            InitializeComponent();

            g = pictureBox1.CreateGraphics();

            ContainerSize = g.VisibleClipBounds.Size.ToSize();
            Rectangle p = new Rectangle(new Point(0, 0), ContainerSize);
            buff_g = BufferedGraphicsManager.Current.Allocate(g, p);

            var t = new Thread(() => Draw());
            t.Start();

            flagButtonAdd = false;
        }

        private void button_add_MouseClick(object sender, MouseEventArgs e)
        {
            flagButtonAdd = true;
            textBox1.Text = "add";
        }

        private Point RandomSgn()
        {
            Point d = new Point(rnd.Next(-1, 1), rnd.Next(-1, 1));

            while(d.X == 0 && d.Y == 0)
            {
                d = new Point(rnd.Next(-1, 1), rnd.Next(-1, 1));
            }
            return d;
            //if (rnd.Next(-10, 10) > 0)
            //    return 1;
            //return -1;
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!flagButtonAdd) { return; }

           
            Square square = new Square(e.X, e.Y);
            squares.Add(square);

            var t = new Thread(() => addNewCircle(e.X, e.Y));
            t.Start();

            flagButtonAdd = false;
            
        }

        private void addNewCircle(int x, int y)
        {
            while (true)
            {
                Point deltaV = RandomSgn();
                Circle circle = new Circle(
                    x, y,
                    10,
                    deltaV.X, deltaV.Y
                );

                circles.Add(circle);

                Thread.Sleep(3000);
            }
        }

        private bool IsAlive(Circle a)
        {
            if (a.X - a.R <= 0 || a.X + a.R > ContainerSize.Width)
                return false;
            if (a.Y - a.R <= 0 || a.Y + a.R > ContainerSize.Height)
                return false;
            return true;
        }

        void Draw()
        {
            while (true)
            {

                foreach(var to in circles.ToList())
                {
                    if (!IsAlive(to))
                    {
                        circles.Remove(to);
                    }
                }

                buff_g.Graphics.Clear(Color.White);
                foreach (var to in squares)
                {
                    buff_g.Graphics.DrawRectangle(pen,
                        to.X - to.HalfSide, to.Y - to.HalfSide,
                        2 * to.HalfSide, 2 * to.HalfSide
                        );
                }

                //textBox1.Text = circles.Count.ToString();

                foreach (var to in circles)
                {
                    to.Move();

                    //textBox1.Text = to.X.ToString() + ' ' + to.Y.ToString();

                    buff_g.Graphics.DrawEllipse(pen,
                        to.X - to.R, to.Y - to.R,
                        2 * to.R, 2 * to.R
                        );
                }
                buff_g.Render();
                Thread.Sleep(100);
            }
        }
    }
}