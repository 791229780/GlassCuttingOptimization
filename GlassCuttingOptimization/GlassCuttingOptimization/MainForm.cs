using AntdUI;
using GlassCuttingOptimization.Enums;
using GlassCuttingOptimization.Utils;
using GlassCuttingOptimization.Views;
using GlassCuttingOptimization.Views.CustomerView;
using GlassCuttingOptimization.Views.OrderView;
using GlassCuttingOptimization.Views.OriginalView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GlassCuttingOptimization
{
    public partial class MainForm: AntdUI.Window
    {
        SqlSugarHelper _db;
        public MainForm()
        {
            _db = new SqlSugarHelper();
            InitializeComponent();

            //加载菜单
            LoadMenu();
            
            //绑定事件
            BindEventHandler();
        }


        private void BindEventHandler()
        {
            this.menu.Mode = TMenuMode.Horizontal;
            menu.SelectChanged += Menu_SelectChanged;
          
            this.Max();
            this.Text = "玻璃排版优化软件";

            btnOrder.Click += BtnOrder_Click;
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
           var order =  new OrderForm(this);
            order.ShowDialog();
        }
   

        private void Menu_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            var name = e.Value.Tag;
            switch (name)
            {
                case NavigationMenu.OriginalPiece: {

                        var form = new OriginalPieceForm(_db);
                        form.ShowDialog();
                    }
                    break;
                case NavigationMenu.Customer: {
                      var form = new CustomerForm();
                        form.ShowDialog();
                    }
                    break;
                case NavigationMenu.ExitSystem: {
                        this.Close();
                    } break;
                default:
                    break;
            }
        }
        private void LoadMenu(string filter = "")
        {
            menu.Items.Clear();
   
            foreach (var rootItem in DataUtil.MenuItems)
            {
                var rootKey = rootItem.Key.ToLower();
                var rootMenu = new AntdUI.MenuItem { Text = rootItem.Key };
                bool rootVisible = false; // 用于标记是否显示根节点

         
                foreach (var item in rootItem.Value)
                {
                    var childText = item.Text.ToLower();

                    // 如果子节点包含搜索文本
                    if (childText.Contains(filter))
                    {
                        var menuItem = new AntdUI.MenuItem
                        {
                            Text = item.Text,
                            IconSvg = item.IconSvg,
                            Tag = item.Tag,
                        };
                        rootMenu.Sub.Add(menuItem);
                        rootVisible = true; // 如果有子节点包含，则显示根节点
                    }
                }

                // 如果根节点包含搜索文本，或有可见的子节点，则显示根节点
                if (rootKey.Contains(filter) || rootVisible)
                {
                    menu.Items.Add(rootMenu);
                }
            }
        }
    }
}
