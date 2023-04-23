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

        private int dx, dy;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int R { get { return r; } set { r = value; } }
        public int Id { get { return id; } set { id = value; } }

        public Circle(int x, int y, int r = 20, int dx = 0, int dy = 0)
        {   
            this.x = x;
            this.y = y;

            this.r = r;

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
