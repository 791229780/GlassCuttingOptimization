using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AntdUI;

namespace GlassCuttingOptimization.Views
{
    public partial class DataGripViewControl1 : UserControl
    {
        public DataGripViewControl1()
        {
            InitializeComponent();
            InitTableColumns();
            InitTableColumns2();
            InitData();
        }
        private void InitData()
        {
            table_base.AutoSizeColumnsMode = ColumnsMode.Fill;
            table_base.EmptyImage = Properties.Resources.编组;
        }

        private void InitTableColumns()
        {
            table_base.Columns = new ColumnCollection() {
                new AntdUI.Column("Index", "序号", ColumnAlign.Center),
                 new AntdUI.Column("Device_code", "订单号", ColumnAlign.Center),
                 new AntdUI.Column("Device_name", "宽度", ColumnAlign.Center),
                 new AntdUI.Column("Retirement_date", "高度", ColumnAlign.Center),
                 new AntdUI.Column("Approved_by", "数量", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "可旋转", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "客户名称", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "产品名称", ColumnAlign.Center),

                          new AntdUI.Column("Retirement_date", "磨边量", ColumnAlign.Center),
                 new AntdUI.Column("Approved_by", "产品编号", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "异形图", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "架号", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "备注", ColumnAlign.Center),

                 new AntdUI.Column("CellLinks", "链接"){
                       LineBreak = true,
                   }
              };
            table_base.EmptyHeader = true;
            table_base.VisibleHeader = true;
        }


        private void InitTableColumns2()
        {
            table_base2.Columns = new ColumnCollection() {
                new AntdUI.Column("Index", "序号", ColumnAlign.Center),
                 new AntdUI.Column("Device_code", "原片编码", ColumnAlign.Center),
                 new AntdUI.Column("Device_name", "架号", ColumnAlign.Center),
                 new AntdUI.Column("Retirement_date", "宽度", ColumnAlign.Center),
                 new AntdUI.Column("Approved_by", "高度", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "数量", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "修变量", ColumnAlign.Center),
                 new AntdUI.Column("Disposal_reason", "产品名称", ColumnAlign.Center),

                          new AntdUI.Column("Retirement_date", "备注", ColumnAlign.Center),

                 new AntdUI.Column("CellLinks", "链接"){
                       LineBreak = true,
                   }
              };
            table_base2.EmptyHeader = true;
            table_base2.VisibleHeader = true;
        }
    }
}
