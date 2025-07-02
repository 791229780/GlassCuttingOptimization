using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassCuttingOptimization.Models.Dto;

namespace GlassCuttingOptimization.Views.Controls
{
    public partial class OptimizationMainControl : UserControl
    {
        private OptimizationControlPanel _controlPanel;
        private OptimizationVisualizationControl _visualizationControl;
        private Splitter _splitter;

        public OptimizationMainControl()
        {
            InitializeComponent();
            SetupLayout();
        }

        public void RunOptimization(List<OrderDto> orders, List<OriginalDto> originalSheets)
        {
            _controlPanel.RunOptimization(orders, originalSheets);
        }

        private void SetupLayout()
        {
            this.SuspendLayout();

            // 创建控制面板（左侧）
            _controlPanel = new OptimizationControlPanel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(250, 250, 250)
            };

            // 创建分割器
            _splitter = new Splitter
            {
                Dock = DockStyle.Left,
                Width = 3,
                BackColor = Color.Gray
            };

            // 创建可视化控件（右侧）
            _visualizationControl = new OptimizationVisualizationControl
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // 设置关联
            _controlPanel.SetVisualizationControl(_visualizationControl);

            // 添加控件
            this.Controls.Add(_visualizationControl);
            this.Controls.Add(_splitter);
            this.Controls.Add(_controlPanel);

            this.ResumeLayout(false);
        }

    }
    }
