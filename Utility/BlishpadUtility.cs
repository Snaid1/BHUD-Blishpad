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
            ContentService.FontSize fontSize;
            switch (size)
            {
                case "8":
                    fontSize = ContentService.FontSize.Size8;
                    break;
                case "11":
                    fontSize = ContentService.FontSize.Size11;
                    break;
                case "12":
                    fontSize = ContentService.FontSize.Size12;
                    break;
                case "14":
                    fontSize = ContentService.FontSize.Size14;
                    break;
                case "16":
                    fontSize = ContentService.FontSize.Size16;
                    break;
                case "18":
                    fontSize = ContentService.FontSize.Size18;
                    break;
                case "20":
                    fontSize = ContentService.FontSize.Size20;
                    break;
                case "22":
                    fontSize = ContentService.FontSize.Size22;
                    break;
                case "24":
                    fontSize = ContentService.FontSize.Size24;
                    break;
                case "32":
                    fontSize = ContentService.FontSize.Size32;
                    break;
                case "34":
                    fontSize = ContentService.FontSize.Size34;
                    break;
                case "36":
                    fontSize = ContentService.FontSize.Size36;
                    break;
                default:
                    fontSize = ContentService.FontSize.Size16;
                    break;
            }
            return GameService.Content.GetFont(ContentService.FontFace.Menomonia, fontSize, ContentService.FontStyle.Regular);
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
