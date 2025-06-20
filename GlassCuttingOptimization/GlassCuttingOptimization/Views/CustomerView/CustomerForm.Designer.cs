namespace GlassCuttingOptimization.Views.CustomerView
{
    partial class CustomerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pageHeader1 = new AntdUI.PageHeader();
            this.panel1 = new AntdUI.Panel();
            this.panel2 = new AntdUI.Panel();
            this.stackPanel1 = new AntdUI.StackPanel();
            this.btnQuery = new AntdUI.Button();
            this.inputcustomer_name = new AntdUI.Input();
            this.label2 = new AntdUI.Label();
            this.inputcustomer_code = new AntdUI.Input();
            this.label1 = new AntdUI.Label();
            this.stackPanel2 = new AntdUI.StackPanel();
            this.btnDelete = new AntdUI.Button();
            this.btnImport = new AntdUI.Button();
            this.btnAdd = new AntdUI.Button();
            this.panel3 = new AntdUI.Panel();
            this.pagination = new AntdUI.Pagination();
            this.stackPanel3 = new AntdUI.StackPanel();
            this.btnJump = new AntdUI.Button();
            this.label4 = new AntdUI.Label();
            this.inputPage = new AntdUI.InputNumber();
            this.label3 = new AntdUI.Label();
            this.table_base = new AntdUI.Table();
            this.stackPanel1.SuspendLayout();
            this.stackPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.stackPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader1
            // 
            this.pageHeader1.DividerShow = true;
            this.pageHeader1.DividerThickness = 2F;
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader1.Location = new System.Drawing.Point(0, 0);
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.ShowIcon = true;
            this.pageHeader1.Size = new System.Drawing.Size(943, 40);
            this.pageHeader1.TabIndex = 0;
            this.pageHeader1.Text = "客户信息";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 789);
            this.panel1.TabIndex = 2;
            this.panel1.Text = "panel1";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(933, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 789);
            this.panel2.TabIndex = 3;
            this.panel2.Text = "panel2";
            // 
            // stackPanel1
            // 
            this.stackPanel1.Controls.Add(this.btnQuery);
            this.stackPanel1.Controls.Add(this.inputcustomer_name);
            this.stackPanel1.Controls.Add(this.label2);
            this.stackPanel1.Controls.Add(this.inputcustomer_code);
            this.stackPanel1.Controls.Add(this.label1);
            this.stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stackPanel1.Location = new System.Drawing.Point(10, 40);
            this.stackPanel1.Name = "stackPanel1";
            this.stackPanel1.Size = new System.Drawing.Size(923, 56);
            this.stackPanel1.TabIndex = 4;
            this.stackPanel1.Text = "stackPanel1";
            // 
            // btnQuery
            // 
            this.btnQuery.IconSvg = "SearchOutlined";
            this.btnQuery.Location = new System.Drawing.Point(587, 3);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(100, 50);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "查询";
            this.btnQuery.Type = AntdUI.TTypeMini.Primary;
            // 
            // inputcustomer_name
            // 
            this.inputcustomer_name.Location = new System.Drawing.Point(381, 3);
            this.inputcustomer_name.Name = "inputcustomer_name";
            this.inputcustomer_name.Size = new System.Drawing.Size(200, 50);
            this.inputcustomer_name.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(295, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 50);
            this.label2.TabIndex = 2;
            this.label2.Text = "客户信息";
            // 
            // inputcustomer_code
            // 
            this.inputcustomer_code.Location = new System.Drawing.Point(89, 3);
            this.inputcustomer_code.Name = "inputcustomer_code";
            this.inputcustomer_code.Size = new System.Drawing.Size(200, 50);
            this.inputcustomer_code.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "客户编码";
            // 
            // stackPanel2
            // 
            this.stackPanel2.Controls.Add(this.btnDelete);
            this.stackPanel2.Controls.Add(this.btnImport);
            this.stackPanel2.Controls.Add(this.btnAdd);
            this.stackPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.stackPanel2.Location = new System.Drawing.Point(10, 96);
            this.stackPanel2.Name = "stackPanel2";
            this.stackPanel2.Size = new System.Drawing.Size(923, 56);
            this.stackPanel2.TabIndex = 5;
            this.stackPanel2.Text = "stackPanel2";
            // 
            // btnDelete
            // 
            this.btnDelete.IconSvg = "DeleteRowOutlined";
            this.btnDelete.Location = new System.Drawing.Point(255, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 50);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "批量删除";
            this.btnDelete.Type = AntdUI.TTypeMini.Primary;
            // 
            // btnImport
            // 
            this.btnImport.IconSvg = "ImportOutlined";
            this.btnImport.Location = new System.Drawing.Point(129, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 50);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "批量导入";
            this.btnImport.Type = AntdUI.TTypeMini.Primary;
            // 
            // btnAdd
            // 
            this.btnAdd.IconSvg = "FileAddOutlined";
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 50);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "新增数据";
            this.btnAdd.Type = AntdUI.TTypeMini.Primary;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pagination);
            this.panel3.Controls.Add(this.stackPanel3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 784);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(923, 45);
            this.panel3.TabIndex = 7;
            this.panel3.Text = "panel3";
            // 
            // pagination
            // 
            this.pagination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagination.Location = new System.Drawing.Point(0, 0);
            this.pagination.Name = "pagination";
            this.pagination.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pagination.Size = new System.Drawing.Size(613, 45);
            this.pagination.TabIndex = 5;
            this.pagination.Text = "pagination1";
            // 
            // stackPanel3
            // 
            this.stackPanel3.Controls.Add(this.btnJump);
            this.stackPanel3.Controls.Add(this.label4);
            this.stackPanel3.Controls.Add(this.inputPage);
            this.stackPanel3.Controls.Add(this.label3);
            this.stackPanel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.stackPanel3.Location = new System.Drawing.Point(613, 0);
            this.stackPanel3.Name = "stackPanel3";
            this.stackPanel3.Size = new System.Drawing.Size(310, 45);
            this.stackPanel3.TabIndex = 3;
            this.stackPanel3.Text = "stackPanel3";
            // 
            // btnJump
            // 
            this.btnJump.Location = new System.Drawing.Point(216, 3);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(80, 39);
            this.btnJump.TabIndex = 3;
            this.btnJump.Text = "跳转";
            this.btnJump.Type = AntdUI.TTypeMini.Primary;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(190, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 39);
            this.label4.TabIndex = 2;
            this.label4.Text = "页";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputPage
            // 
            this.inputPage.Location = new System.Drawing.Point(84, 3);
            this.inputPage.Name = "inputPage";
            this.inputPage.Size = new System.Drawing.Size(100, 39);
            this.inputPage.TabIndex = 1;
            this.inputPage.Text = "0";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "跳转至第";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // table_base
            // 
            this.table_base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_base.Location = new System.Drawing.Point(10, 152);
            this.table_base.Name = "table_base";
            this.table_base.Size = new System.Drawing.Size(923, 632);
            this.table_base.TabIndex = 8;
            this.table_base.Text = "table1";
            // 
            // CustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(943, 829);
            this.Controls.Add(this.table_base);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.stackPanel2);
            this.Controls.Add(this.stackPanel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pageHeader1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CustomerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomerForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.stackPanel1.ResumeLayout(false);
            this.stackPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.stackPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.StackPanel stackPanel1;
        private AntdUI.StackPanel stackPanel2;
        private AntdUI.Label label1;
        private AntdUI.Input inputcustomer_code;
        private AntdUI.Button btnQuery;
        private AntdUI.Input inputcustomer_name;
        private AntdUI.Label label2;
        private AntdUI.Button btnAdd;
        private AntdUI.Button btnDelete;
        private AntdUI.Button btnImport;
        private AntdUI.Panel panel3;
        private AntdUI.Pagination pagination;
        private AntdUI.StackPanel stackPanel3;
        private AntdUI.Label label3;
        private AntdUI.InputNumber inputPage;
        private AntdUI.Label label4;
        private AntdUI.Button btnJump;
        private AntdUI.Table table_base;
    }
}