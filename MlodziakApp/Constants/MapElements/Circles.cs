using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Constants.MapElements
{
    public class Circles
    {
        public static readonly Color NotVisited = Color.FromRgba(178, 102, 255, 128);
        public static readonly Color Visited = Color.FromRgba(89, 19, 89, 128);
        public static readonly Color Ommitted = Color.FromRgba(93, 92, 97, 128);
        public static readonly Color StartsSoon = Color.FromRgba(227, 215, 45, 128);
        public static readonly Color Default = Color.FromRgba(0, 0, 0, 128);

    }
}
