using GlassCuttingOptimization.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Utils
{
    public class MenuItems
    {
        public string IconSvg { get; set; } = null;
        public string Text { get; set; } = string.Empty;
        public NavigationMenu Tag { get; set; }
    }
}
