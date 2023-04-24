using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace balls_and_rectangle
{
    internal class Circle
    {   
        private int x, y;
        private int r;
        private int id;

        private int idParent = -1;

        private static int cnt = 0;

        private Color color = Color.Black;
        private int dx, dy;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int R { get { return r; } set { r = value; } }
        public int Id { get { return id; } set { id = value; } }

        public int Parent { get { return idParent; } set { idParent = value; } }
        public Color GetColor { get { return color; } set { color = value; } }
        public Circle(int x, int y, int r = 20, int dx = 0, int dy = 0, int R = 0, int G = 0, int B = 0, int idParnet = -1)
        {   
            this.x = x;
            this.y = y;

            this.r = r;

            this.id = cnt;
            cnt += 1;

            this.idParent = idParnet;

            this.color = Color.FromArgb(R, G, B);

            this.dx = dx*5;
            this.dy = dy*5;
        }

        public void Move()
        {   
            this.x += dx;    
            this.y += dy;
        }
    }
}
