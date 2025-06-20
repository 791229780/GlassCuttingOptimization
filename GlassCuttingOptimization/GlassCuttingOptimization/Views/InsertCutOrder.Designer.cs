namespace GlassCuttingOptimization.Views
{
    partial class InsertCutOrder
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
            this.windowBar = new AntdUI.PageHeader();
            this.SuspendLayout();
            // 
            // windowBar
            // 
            this.windowBar.BackColor = System.Drawing.Color.White;
            this.windowBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.windowBar.Location = new System.Drawing.Point(0, 0);
            this.windowBar.Name = "windowBar";
            this.windowBar.Size = new System.Drawing.Size(898, 40);
            this.windowBar.TabIndex = 0;
            this.windowBar.Text = "pageHeader1";
            // 
            // InsertCutOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(898, 587);
            this.Controls.Add(this.windowBar);
            this.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "InsertCutOrder";
            this.Text = "InsertCutOrder";
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader windowBar;
    }
}