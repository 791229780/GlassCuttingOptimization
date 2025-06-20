using GlassCuttingOptimization.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Utils
{
    public class DataUtil
    {
        public static readonly Dictionary<string, List<MenuItems>> MenuItems = new Dictionary<string, List<MenuItems>>()
        {
                { "文件(F)", new List<MenuItems>
        {
              new MenuItems { Text = "原片库存" , Tag = NavigationMenu.OriginalPiece},
               new MenuItems { Text = "退出系统" , Tag = NavigationMenu.ExitSystem},
        }
        },
                { "系统(S)", new List<MenuItems>
        {
               new MenuItems { Text = "设置" , Tag = NavigationMenu.Settings},
        }
        },
               { "数据(D)", new List<MenuItems>
        {
            new MenuItems { Text = "客户信息" , Tag = NavigationMenu.Customer},
            new MenuItems { Text = "导出G代码" , Tag =NavigationMenu.ExportG},
        }
        },
               { "打印(P)", new List<MenuItems>
        {
               new MenuItems { Text = "报表" , Tag =NavigationMenu.Report},
           
        }
        },
               { "帮助(H)", new List<MenuItems>
        {
               new MenuItems { Text = "关于" , Tag = NavigationMenu.About},
        }
        }
        };

    }
}
