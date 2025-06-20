using AntdUI;
using GlassCuttingOptimization.Models.Model;
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

namespace GlassCuttingOptimization.Views.OrderView
{
    public partial class NoteForm : AntdUI.Window
    {

        public event EventHandler<Tuple<NoteModel>> OnModel_Event;
        public NoteForm(NoteModel noteModel)
        {
            InitializeComponent();

            InitData(noteModel);
            //绑定事件
            BindEventHandler();
        }
        private void BindEventHandler()
        {
            btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var dto = new NoteModel();
            dto.Note1 = inputNote1.Text;
            dto.Note2 = inputNote2.Text;
            dto.Note3 = inputNote3.Text;
            dto.Note4 = inputNote4.Text;
            dto.Note5 = inputNote5.Text;
            dto.Note6 = inputNote6.Text;
            dto.Note7 = inputNote7.Text;
            dto.Note8 = inputNote8.Text;
            dto.Note9 = inputNote9.Text;
            dto.Note10 = inputNote10.Text;
            dto.Note11 = inputNote11.Text;
            dto.Note12 = inputNote12.Text;
            dto.Note13 = inputNote13.Text;
            dto.Note14 = inputNote14.Text;
            dto.Note15 = inputNote15.Text;
            dto.Note16 = inputNote16.Text;
            dto.Note17 = inputNote17.Text;
            dto.Note18 = inputNote18.Text;
            dto.Note19 = inputNote19.Text;
            dto.Note20 = inputNote20.Text;

            dto.Data1 = inputData1.Text;
            dto.Data2 = inputData2.Text;
            dto.Data3 = inputData3.Text;
            dto.Data4 = inputData4.Text;
            dto.Data5 = inputData5.Text;
            dto.Data6 = inputData6.Text;
            dto.Data7 = inputData7.Text;
            dto.Data8 = inputData8.Text;
            dto.Data9 = inputData9.Text;
            dto.Data10 = inputData10.Text;
            OnModel_Event?.Invoke(OnModel_Event, new Tuple<NoteModel>(dto));
            this.Close();
        }

        private void InitData(NoteModel noteModel)
        {
            this.windowBar.Text = "附加注释";
            this.windowBar.DividerShow = true;
            this.windowBar.DividerThickness = 2;
            this.windowBar.ShowIcon = true;
            this.windowBar.ShowButton = true;
            this.StartPosition = FormStartPosition.CenterScreen;


            this.inputNote1.Text = noteModel.Note1;
            this.inputNote2.Text = noteModel.Note2;
            this.inputNote3.Text = noteModel.Note3;
            this.inputNote4.Text = noteModel.Note4;
            this.inputNote5.Text = noteModel.Note5;
            this.inputNote6.Text = noteModel.Note6;
            this.inputNote7.Text = noteModel.Note7;
            this.inputNote8.Text = noteModel.Note8;
            this.inputNote9.Text = noteModel.Note9;
            this.inputNote10.Text = noteModel.Note10;
            this.inputNote11.Text = noteModel.Note11;
            this.inputNote12.Text = noteModel.Note12;
            this.inputNote13.Text = noteModel.Note13;
            this.inputNote14.Text = noteModel.Note14;
            this.inputNote15.Text = noteModel.Note15;
            this.inputNote16.Text = noteModel.Note16;
            this.inputNote17.Text = noteModel.Note17;
            this.inputNote18.Text = noteModel.Note18;
            this.inputNote19.Text = noteModel.Note19;
            this.inputNote20.Text = noteModel.Note20;

            this.inputData1.Text = noteModel.Data1;
            this.inputData2.Text = noteModel.Data2;
            this.inputData3.Text = noteModel.Data3;
            this.inputData4.Text = noteModel.Data4;
            this.inputData5.Text = noteModel.Data5;
            this.inputData6.Text = noteModel.Data6;
            this.inputData7.Text = noteModel.Data7;
            this.inputData8.Text = noteModel.Data8;
            this.inputData9.Text = noteModel.Data9;
            this.inputData10.Text = noteModel.Data10;
        }

        private void NoteForm_Load(object sender, EventArgs e)
        {

        }
    }
}
