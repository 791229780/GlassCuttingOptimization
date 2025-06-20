namespace GlassCuttingOptimization.Views
{
    partial class DataGripViewControl1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new AntdUI.Panel();
            this.pageHeader2 = new AntdUI.PageHeader();
            this.table_base2 = new AntdUI.Table();
            this.panel2 = new AntdUI.Panel();
            this.pageHeader1 = new AntdUI.PageHeader();
            this.table_base = new AntdUI.Table();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.table_base2);
            this.panel1.Controls.Add(this.pageHeader2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 394);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(874, 166);
            this.panel1.TabIndex = 1;
            this.panel1.Text = "panel1";
            // 
            // pageHeader2
            // 
            this.pageHeader2.DividerShow = true;
            this.pageHeader2.DividerThickness = 2F;
            this.pageHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader2.Location = new System.Drawing.Point(0, 0);
            this.pageHeader2.Name = "pageHeader2";
            this.pageHeader2.Size = new System.Drawing.Size(874, 40);
            this.pageHeader2.TabIndex = 1;
            this.pageHeader2.Text = "原片列表";
            // 
            // table_base2
            // 
            this.table_base2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_base2.Location = new System.Drawing.Point(0, 40);
            this.table_base2.Name = "table_base2";
            this.table_base2.Size = new System.Drawing.Size(874, 126);
            this.table_base2.TabIndex = 2;
            this.table_base2.Text = "table1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.table_base);
            this.panel2.Controls.Add(this.pageHeader1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(874, 394);
            this.panel2.TabIndex = 2;
            this.panel2.Text = "panel2";
            // 
            // pageHeader1
            // 
            this.pageHeader1.DividerShow = true;
            this.pageHeader1.DividerThickness = 2F;
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader1.Location = new System.Drawing.Point(0, 0);
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.Size = new System.Drawing.Size(874, 40);
            this.pageHeader1.TabIndex = 1;
            this.pageHeader1.Text = "成品列表";
            // 
            // table_base
            // 
            this.table_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_base.Location = new System.Drawing.Point(0, 40);
            this.table_base.Name = "table_base";
            this.table_base.Size = new System.Drawing.Size(874, 354);
            this.table_base.TabIndex = 2;
            this.table_base.Text = "table2";
            // 
            // DataGripViewControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DataGripViewControl1";
            this.Size = new System.Drawing.Size(874, 560);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.Panel panel1;
        private AntdUI.PageHeader pageHeader2;
        private AntdUI.Table table_base2;
        private AntdUI.Panel panel2;
        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Table table_base;
    }
}
