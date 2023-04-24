using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Xml.Linq;

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
            Point d = new Point(rnd.Next(-1, 2), rnd.Next(-1, 2));

            while (d.X == 0 && d.Y == 0)
            {
                d = new Point(rnd.Next(-1, 2), rnd.Next(-1, 2));
            }
            return d;
            //if (rnd.Next(-10, 10) > 0)
            //    return 1;
            //return -1;
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!flagButtonAdd) { return; }


            Square square = new Square(
                e.X, e.Y,
                14,
                rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            squares.Add(square);

            var t = new Thread(() => addNewCircle(e.X, e.Y, square.GetColor, square.Id));
            t.Start();

            flagButtonAdd = false;

        }

        bool IsAliveSquare(int id)
        {
            if(squares.Count == 0) return false;

            foreach (var square in squares)
            {
                if (square.Id == id) return true;
            }
            return false;
        }
        private void addNewCircle(int x, int y, Color color, int id)
        {
            while (IsAliveSquare(id))
            {
                Point deltaV = RandomSgn();
                Circle circle = new Circle(
                    x, y,
                    10,
                    deltaV.X, deltaV.Y,
                    color.R, color.G, color.B,
                    id
                );

                circles.Add(circle);

                Thread.Sleep(3000);
            }
        }

        private bool IsAliveCircle(Circle a)
        {
            if (a.X - a.R <= 0 || a.X + a.R > ContainerSize.Width)
                return false;
            if (a.Y - a.R <= 0 || a.Y + a.R > ContainerSize.Height)
                return false;
            return true;
        }

        private bool IsIntersectionCircle(Circle a, Circle b)
        {
            if ((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) < a.R * a.R * 4)
                return true;
            return false;
        }

        void Draw()
        {
            while (true)
            {
                List<Circle> circlesCopy;
                if (circles.Count > 0)
                {
                    // удаляем окружности, которые вышли за пределы
                    circlesCopy = circles.ToList();
                    foreach (var to in circlesCopy)
                    {
                        if (!IsAliveCircle(to))
                        {
                            circles.Remove(to);
                        }
                    }
                }

                if (circles.Count > 0)
                {
                    circlesCopy = circles.ToList();

                    // находим окружности, которые пересекаются
                    List<int> removeCircles = new List<int>();
                                        
                    for (int i = 0; i < circlesCopy.Count; ++i)
                    {
                        for (int j = i + 1; j < circlesCopy.Count; ++j)
                        {
                            if (IsIntersectionCircle(circlesCopy[i], circlesCopy[j]))
                            {
                                if (rnd.Next(0, 10) % 2 == 0)
                                {
                                    removeCircles.Add(i);
                                    for(int k = 0; k < squares.Count; ++k)
                                    {
                                        if (squares[k].Id == circlesCopy[j].Parent)
                                            squares[k].CntKill += 1;
                                    }
                                }
                                else
                                {
                                    removeCircles.Add(j);
                                    for (int k = 0; k < squares.Count; ++k)
                                    {
                                        if (squares[k].Id == circlesCopy[i].Parent)
                                            squares[k].CntKill += 1;
                                    }
                                }
                                
                                break;
                            }
                        }
                    }

                    // удаляем окружности, которые пересекаются
                    foreach (var to in removeCircles)
                    {
                        if (circles.Count > 0)
                            circles.Remove(circlesCopy[to]);
                    }
                }



                // рисуем квадраты
                buff_g.Graphics.Clear(Color.White);

                if (squares.Count > 0)
                {
                    foreach (var to in squares)
                    {
                        pen.Color = to.GetColor;


                        string text1 = to.CntKill.ToString();
                        using (Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point))
                        {
                            RectangleF tmp = new Rectangle(
                                to.X - to.HalfSide, to.Y - to.HalfSide,
                                2 * to.HalfSide, 2 * to.HalfSide);

                            buff_g.Graphics.DrawString(text1, font1, Brushes.Black, tmp);
                            buff_g.Graphics.DrawRectangle(pen, Rectangle.Round(tmp));
                        }
                        
                    }
                }


                //textBox1.Text = circles.Count.ToString();
                // рисуем окружности 
                if (circles.Count > 0)
                {

                    foreach (var to in circles)
                    {
                        to.Move();

                        pen.Color = to.GetColor;
                        buff_g.Graphics.DrawEllipse(pen,
                            to.X - to.R, to.Y - to.R,
                            2 * to.R, 2 * to.R
                            );
                    }
                }
                buff_g.Render();
                Thread.Sleep(100);
            }
        }

        private void clear_MouseClick(object sender, MouseEventArgs e)
        {
            buff_g.Graphics.Clear(Color.White);
            buff_g.Render();

            circles.Clear();
            squares.Clear();
        }
    }
}