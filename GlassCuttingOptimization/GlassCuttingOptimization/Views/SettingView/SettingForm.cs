using AntdUI;
using GlassCuttingOptimization.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Views.SettingView
{
    public partial class SettingForm: Window
    {
        private IniFileHelper _iniFileHelper;
        public SettingForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            BindEventHandler();
        }
       
        private async void BindEventHandler()
        {
            var path = Path.Combine(Application.StartupPath, "config.ini");
            _iniFileHelper = new IniFileHelper(path);
            inputPath.Text = _iniFileHelper.Read("Settings", "SaveFile", "");
            btnSelect.Click += BtnSelect_Click;
        }
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            selectFile();


        }

        private void selectFile()
        {

            using (System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取选择的路径
                    string selectedPath = folderDialog.SelectedPath;
                    inputPath.Text = selectedPath;

                    _iniFileHelper.Write("Settings", "SaveFile", selectedPath);
                }
            }
        }

    }
}
