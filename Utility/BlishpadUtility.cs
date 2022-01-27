using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snaid1.Blishpad.Utility
{
    class BlishpadUtility
    {

        public static int ScaleInt(int num, float scale)
        {
            var newNum = Convert.ToInt32(num * scale);
            return newNum;
        }
        public static Point ScalePoint(Point point, float scale)
        {
            var newPoint = new Point(ScaleInt(point.X, scale), ScaleInt(point.Y, scale));
            return newPoint;
        }

        public static Rectangle ScaleRectangle(Rectangle rect, float scale)
        {
            var newRect = new Rectangle(ScaleInt(rect.X, scale), ScaleInt(rect.Y, scale), ScaleInt(rect.Width, scale), ScaleInt(rect.Height, scale));
            return newRect;
        }
    }
}
