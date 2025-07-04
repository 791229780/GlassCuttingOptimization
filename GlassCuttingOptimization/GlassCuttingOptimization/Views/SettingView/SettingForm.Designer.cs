namespace GlassCuttingOptimization.Views.SettingView
{
    partial class SettingForm
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
            this.stackPanel1 = new AntdUI.StackPanel();
            this.label1 = new AntdUI.Label();
            this.inputPath = new AntdUI.Input();
            this.btnSelect = new AntdUI.Button();
            this.stackPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader1
            // 
            this.pageHeader1.BackColor = System.Drawing.Color.White;
            this.pageHeader1.DividerShow = true;
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader1.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader1.Location = new System.Drawing.Point(0, 0);
            this.pageHeader1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.ShowIcon = true;
            this.pageHeader1.Size = new System.Drawing.Size(611, 30);
            this.pageHeader1.TabIndex = 1;
            this.pageHeader1.Text = "G代码路径设置";
            // 
            // stackPanel1
            // 
            this.stackPanel1.Controls.Add(this.btnSelect);
            this.stackPanel1.Controls.Add(this.inputPath);
            this.stackPanel1.Controls.Add(this.label1);
            this.stackPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.stackPanel1.Location = new System.Drawing.Point(0, 30);
            this.stackPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.stackPanel1.Name = "stackPanel1";
            this.stackPanel1.Size = new System.Drawing.Size(611, 58);
            this.stackPanel1.TabIndex = 2;
            this.stackPanel1.Text = "stackPanel1";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "保存路径";
            // 
            // inputPath
            // 
            this.inputPath.Location = new System.Drawing.Point(99, 3);
            this.inputPath.Name = "inputPath";
            this.inputPath.Size = new System.Drawing.Size(390, 52);
            this.inputPath.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(495, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(88, 52);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "选择路径";
            this.btnSelect.Type = AntdUI.TTypeMini.Primary;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 94);
            this.Controls.Add(this.stackPanel1);
            this.Controls.Add(this.pageHeader1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "SettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingForm";
            this.stackPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader1;
        private AntdUI.StackPanel stackPanel1;
        private AntdUI.Label label1;
        private AntdUI.Button btnSelect;
        private AntdUI.Input inputPath;
    }
}