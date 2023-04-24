using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace balls_and_rectangle
{
    internal class Square
    {
        private int x, y;
        private int d;
        private int id;

        private Color color;

        private int cntKill = 0;

        private static int cnt = 0;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int HalfSide { get { return d; } set { d = value; } }
        public int Id { get { return id; } set { id = value; } }

        public int CntKill { get { return cntKill; } set { cntKill = value; } }

        public Color GetColor { get { return color; } set { color = value; } }
        public Square(int _x = 0, int _y = 0, int _d = 14, int r = 0, int g = 0, int b = 0) {
            x = _x;
            y = _y;
            d = _d;

            this.color = Color.FromArgb(r, g, b);

            this.id = cnt;
            cnt++;
        }
    }
}
