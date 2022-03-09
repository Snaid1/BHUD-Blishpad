using Blish_HUD;
using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
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

        public static BitmapFont getFont(string size)
        {
            BitmapFont font;

            switch (size)
            {
                case "12":
                    font = GameService.Content.DefaultFont12;
                    break;
                case "14":
                    font = GameService.Content.DefaultFont14;
                    break;
                case "16":
                    font = GameService.Content.DefaultFont16;
                    break;
                case "18":
                    font = GameService.Content.DefaultFont18;
                    break;
                case "32":
                    font = GameService.Content.DefaultFont32;
                    break;
                default:
                    font = GameService.Content.DefaultFont16;
                    break;
            }
            return font;
        }


        private int calcHeight(Control cont, int max)
        {
            var newmax = max;
            if (newmax < cont.Height)
            {
                newmax = cont.Height;
            }
            if (cont is Container)
            {
                Container container = (Container)cont;
                var childrenHeight = 0;
                foreach (Control child in container.Children)
                {
                    childrenHeight += calcHeight(child, newmax);
                }
                if (newmax < childrenHeight) { newmax = childrenHeight; }
            }
            return newmax;
        }
    }
}
