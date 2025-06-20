using GlassCuttingOptimization.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Utils
{
    public class DataOriginalUtil
    {
        public static readonly Dictionary<string, List<MenuItems>> MenuItems = new Dictionary<string, List<MenuItems>>()
        {
                { "文件", new List<MenuItems>
        {

                 new MenuItems { Text = "关闭" , Tag = NavigationMenu.ExitSystem},
        }
        },
                { "库存", new List<MenuItems>
        {
               new MenuItems { Text = "新材料" , Tag = NavigationMenu.AddMaterials},
                 new MenuItems { Text = "移除材料" , Tag = NavigationMenu.DelMaterials},

                     new MenuItems { Text = "新原片格式" , Tag = NavigationMenu.AddOriginal},
                 new MenuItems { Text = "移除原片格式" , Tag = NavigationMenu.DelOriginal},
        }
        }
        };
    }

    public class DataOrderUtil
    {
        public static readonly Dictionary<string, List<MenuItems>> MenuItems = new Dictionary<string, List<MenuItems>>()
        {
                { "文件", new List<MenuItems>
        {

                 new MenuItems { Text = "关闭" , Tag = NavigationMenu.ExitSystem},
        }
        },
                { "最优化", new List<MenuItems>
        {
          new MenuItems { Text = "优化" , Tag = NavigationMenu.Optimization},

        }
        },
                { "件数", new List<MenuItems>
        {
     new MenuItems { Text = "新空行" , Tag = NavigationMenu.AddOrder},
          new MenuItems { Text = "删除小片" , Tag = NavigationMenu.DelOrder},
        }
        }
        };
    }
}
