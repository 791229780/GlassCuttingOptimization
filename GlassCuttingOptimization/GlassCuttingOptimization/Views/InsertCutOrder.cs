using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Views
{
    public partial class InsertCutOrder: Window
    {
        public InsertCutOrder()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            this.windowBar.Text = "插入切割订单";
            this.windowBar.DividerShow = true;
            this.windowBar.DividerThickness = 2;
            this.windowBar.ShowIcon = true;
            this.windowBar.ShowButton = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
