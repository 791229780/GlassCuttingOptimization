namespace GlassCuttingOptimization.Views.Controls
{
    partial class OptimizationControlPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btnOptimize = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTotalSheets = new System.Windows.Forms.Label();
            this.lblOverallUtilization = new System.Windows.Forms.Label();
            this.lblAlgorithmUsed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSheetSelector = new System.Windows.Forms.Label();
            this.cmbSheetSelector = new System.Windows.Forms.ComboBox();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblZoom = new System.Windows.Forms.Label();
            this.btnZoomFit = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.trackBarZoom = new System.Windows.Forms.TrackBar();
            this.btnGenerateGCode = new System.Windows.Forms.Button();
            this.btnPreviewGCode = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAlgorithm);
            this.panel1.Controls.Add(this.cmbAlgorithm);
            this.panel1.Controls.Add(this.btnOptimize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 60);
            this.panel1.TabIndex = 0;
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Location = new System.Drawing.Point(10, 10);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(59, 12);
            this.lblAlgorithm.TabIndex = 0;
            this.lblAlgorithm.Text = "优化算法:";
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(80, 7);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(200, 20);
            this.cmbAlgorithm.TabIndex = 1;
            // 
            // btnOptimize
            // 
            this.btnOptimize.Location = new System.Drawing.Point(80, 33);
            this.btnOptimize.Name = "btnOptimize";
            this.btnOptimize.Size = new System.Drawing.Size(75, 23);
            this.btnOptimize.TabIndex = 2;
            this.btnOptimize.Text = "开始优化";
            this.btnOptimize.UseVisualStyleBackColor = true;
            this.btnOptimize.Click += new System.EventHandler(this.btnOptimize_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblTotalSheets);
            this.panel2.Controls.Add(this.lblOverallUtilization);
            this.panel2.Controls.Add(this.lblAlgorithmUsed);
            this.panel2.Controls.Add(this.lblStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 140);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 100);
            this.panel2.TabIndex = 1;
            // 
            // lblTotalSheets
            // 
            this.lblTotalSheets.AutoSize = true;
            this.lblTotalSheets.Location = new System.Drawing.Point(10, 10);
            this.lblTotalSheets.Name = "lblTotalSheets";
            this.lblTotalSheets.Size = new System.Drawing.Size(71, 12);
            this.lblTotalSheets.TabIndex = 0;
            this.lblTotalSheets.Text = "总板材数: 0";
            // 
            // lblOverallUtilization
            // 
            this.lblOverallUtilization.AutoSize = true;
            this.lblOverallUtilization.Location = new System.Drawing.Point(10, 30);
            this.lblOverallUtilization.Name = "lblOverallUtilization";
            this.lblOverallUtilization.Size = new System.Drawing.Size(101, 12);
            this.lblOverallUtilization.TabIndex = 1;
            this.lblOverallUtilization.Text = "整体利用率: 0.0%";
            // 
            // lblAlgorithmUsed
            // 
            this.lblAlgorithmUsed.AutoSize = true;
            this.lblAlgorithmUsed.Location = new System.Drawing.Point(10, 50);
            this.lblAlgorithmUsed.Name = "lblAlgorithmUsed";
            this.lblAlgorithmUsed.Size = new System.Drawing.Size(41, 12);
            this.lblAlgorithmUsed.TabIndex = 2;
            this.lblAlgorithmUsed.Text = "算法: ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(10, 70);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 12);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "就绪状态";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnPreviewGCode);
            this.panel3.Controls.Add(this.btnGenerateGCode);
            this.panel3.Controls.Add(this.lblSheetSelector);
            this.panel3.Controls.Add(this.cmbSheetSelector);
            this.panel3.Controls.Add(this.btnRotate);
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 240);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 100);
            this.panel3.TabIndex = 2;
            // 
            // lblSheetSelector
            // 
            this.lblSheetSelector.AutoSize = true;
            this.lblSheetSelector.Location = new System.Drawing.Point(10, 10);
            this.lblSheetSelector.Name = "lblSheetSelector";
            this.lblSheetSelector.Size = new System.Drawing.Size(59, 12);
            this.lblSheetSelector.TabIndex = 0;
            this.lblSheetSelector.Text = "选择板材:";
            // 
            // cmbSheetSelector
            // 
            this.cmbSheetSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSheetSelector.FormattingEnabled = true;
            this.cmbSheetSelector.Location = new System.Drawing.Point(80, 7);
            this.cmbSheetSelector.Name = "cmbSheetSelector";
            this.cmbSheetSelector.Size = new System.Drawing.Size(200, 20);
            this.cmbSheetSelector.TabIndex = 1;
            this.cmbSheetSelector.SelectedIndexChanged += new System.EventHandler(this.cmbSheetSelector_SelectedIndexChanged);
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(80, 40);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(75, 23);
            this.btnRotate.TabIndex = 2;
            this.btnRotate.Text = "旋转选中";
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(170, 40);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "重置排版";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblZoom);
            this.panel4.Controls.Add(this.btnZoomFit);
            this.panel4.Controls.Add(this.btnZoomOut);
            this.panel4.Controls.Add(this.btnZoomIn);
            this.panel4.Controls.Add(this.trackBarZoom);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(300, 80);
            this.panel4.TabIndex = 3;
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = true;
            this.lblZoom.Location = new System.Drawing.Point(10, 10);
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(35, 12);
            this.lblZoom.TabIndex = 0;
            this.lblZoom.Text = "缩放:";
            // 
            // btnZoomFit
            // 
            this.btnZoomFit.Location = new System.Drawing.Point(60, 7);
            this.btnZoomFit.Name = "btnZoomFit";
            this.btnZoomFit.Size = new System.Drawing.Size(50, 23);
            this.btnZoomFit.TabIndex = 1;
            this.btnZoomFit.Text = "适应";
            this.btnZoomFit.UseVisualStyleBackColor = true;
            this.btnZoomFit.Click += new System.EventHandler(this.btnZoomFit_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Location = new System.Drawing.Point(115, 7);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(30, 23);
            this.btnZoomOut.TabIndex = 2;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Location = new System.Drawing.Point(150, 7);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(30, 23);
            this.btnZoomIn.TabIndex = 3;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // trackBarZoom
            // 
            this.trackBarZoom.Location = new System.Drawing.Point(10, 35);
            this.trackBarZoom.Maximum = 500;
            this.trackBarZoom.Minimum = 10;
            this.trackBarZoom.Name = "trackBarZoom";
            this.trackBarZoom.Size = new System.Drawing.Size(270, 45);
            this.trackBarZoom.TabIndex = 4;
            this.trackBarZoom.Value = 100;
            this.trackBarZoom.ValueChanged += new System.EventHandler(this.trackBarZoom_ValueChanged);
            // 
            // btnGenerateGCode
            // 
            this.btnGenerateGCode.Location = new System.Drawing.Point(80, 70);
            this.btnGenerateGCode.Name = "btnGenerateGCode";
            this.btnGenerateGCode.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateGCode.TabIndex = 4;
            this.btnGenerateGCode.Text = "G代码生成";
            this.btnGenerateGCode.UseVisualStyleBackColor = true;
            this.btnGenerateGCode.Click += new System.EventHandler(this.btnGenerateGCode_Click);
            // 
            // btnPreviewGCode
            // 
            this.btnPreviewGCode.Location = new System.Drawing.Point(170, 69);
            this.btnPreviewGCode.Name = "btnPreviewGCode";
            this.btnPreviewGCode.Size = new System.Drawing.Size(75, 23);
            this.btnPreviewGCode.TabIndex = 5;
            this.btnPreviewGCode.Text = "预览G代码";
            this.btnPreviewGCode.UseVisualStyleBackColor = true;
            // 
            // OptimizationControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Name = "OptimizationControlPanel";
            this.Size = new System.Drawing.Size(300, 500);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAlgorithm;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Button btnOptimize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTotalSheets;
        private System.Windows.Forms.Label lblOverallUtilization;
        private System.Windows.Forms.Label lblAlgorithmUsed;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblSheetSelector;
        private System.Windows.Forms.ComboBox cmbSheetSelector;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Button btnReset; // 新增重置按钮
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblZoom;
        private System.Windows.Forms.Button btnZoomFit;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.TrackBar trackBarZoom;
        private System.Windows.Forms.Button btnGenerateGCode;
        private System.Windows.Forms.Button btnPreviewGCode;
    }
}
