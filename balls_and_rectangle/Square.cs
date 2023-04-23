using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace balls_and_rectangle
{
    internal class Square
    {
        int x, y;
        int d;
        int id;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int HalfSide { get { return d; } set { d = value; } }
        public int Id { get { return id; } set { id = value; } }
        public Square(int _x = 0, int _y = 0, int _d = 20) {
            x = _x;
            y = _y;
            d = _d;
        }
    }
}
