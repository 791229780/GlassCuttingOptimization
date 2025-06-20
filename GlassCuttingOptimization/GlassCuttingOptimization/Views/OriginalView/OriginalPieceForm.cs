using AntdUI;
using DevExpress.Utils;
using GlassCuttingOptimization.Enums;
using GlassCuttingOptimization.Models;
using GlassCuttingOptimization.Models.Dto;
using GlassCuttingOptimization.Models.UIModels;
using GlassCuttingOptimization.Utils;
using GlassCuttingOptimization.Views.CustomerView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace GlassCuttingOptimization.Views.OriginalView
{
    public partial class OriginalPieceForm : Window
    {
        SqlSugarHelper _db;
        MaterialDto _material;
        OriginalDto _original;
        public OriginalPieceForm(SqlSugarHelper db)
        {
            _material = new MaterialDto();
            _original = new OriginalDto();
            _db = db;
            InitializeComponent();
            InitData();
            LoadMenu();
         
            InitTableColumns();
            MaterialTbaleInfo();
            //绑定事件
            BindEventHandler();
        }
        private void BindEventHandler()
        {
            this.menu.Mode = TMenuMode.Horizontal;
            menu.SelectChanged += Menu_SelectChanged;
            Materialtable.EmptyHeader = true;
            OriginalTable.EmptyHeader = true;
            this.Text = "原片库存";

     

            Materialtable.CellButtonClick += Materialtable_CellButtonClick;
            Materialtable.CellClick += Materialtable_CellClick;
            btnMaterialSave.Click += BtnMaterialSave_Click;

            btnAddMaterials.Click += BtnAddMaterials_Click;
            btnDelMaterials.Click += BtnDelMaterials_Click;

       
            OriginalTable.CellButtonClick += OriginalTable_CellButtonClick;
            btnOriginalSave.Click += BtnOriginalSave_Click;
            OriginalTable.CellClick += OriginalTable_CellClick;

            btnAddOriginal.Click += BtnAddOriginal_Click;
            btnDelOriginal.Click += BtnDelOriginal_Click;
        }

        private void BtnDelOriginal_Click(object sender, EventArgs e)
        {
            DelOriginal();


        }

        private void OriginalTable_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                return;
            }
            if (e.Record is UIOriginal dto)
            {
     
                _original = _db.Query<OriginalDto>().Where(c => c.ID == dto.ID).FirstOrDefault();
                inputNumber.Value = dto.Number;
                inputOX.Text = dto.X1;
                inputOY.Text = dto.Y1;
                inputPriority.Value = int.Parse(dto.Priority);
                selectOType.Text = dto.type;

                OriginalTableInfo(_material.ID);
            }
        }

        private void BtnOriginalSave_Click(object sender, EventArgs e)
        {
            _original.Number = long.Parse( inputNumber.Value.ToString());
            _original.X1 = inputOX.Text;
            _original.Y1 = inputOY.Text;
            _original.Priority = inputPriority.Text;
            _original.Type = selectOType.Text;
    

            var relust = _db.Update(_original);
            if (relust > 0)
            {
               OriginalTbaleInfo();
            }
        }

        private void OriginalTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            var buttontext = e.Btn.Text;

            var dto = (UIOriginal)e.Record;
            switch (buttontext)
            {

                case "删除":
                    {
                        var result = AntdUI.Modal.open(this, "删除警告！", "删除后无法恢复！", TType.Warn);
                        if (result != DialogResult.OK)
                        {
                            return;
                        }
                        else
                        {
                            var count = _db.Delete<OriginalDto>(dto.ID);
                            if (count > 0)
                            {

                                OriginalTbaleInfo();
                                AntdUI.Notification.success(
                                             this,
                                             "提示",
                                             "删除成功！",
                                             autoClose: 3,
                                             align: TAlignFrom.TR
                                         );
                            }
                            else
                            {
                                AntdUI.Notification.error(
                                             this,
                                             "提示",
                                             "删除失败！",
                                             autoClose: 3,
                                             align: TAlignFrom.TR
                                         );
                            }
                        }
                        return;
                    }
            }
        }

        private void BtnAddOriginal_Click(object sender, EventArgs e)
        {
            AddOriginal();
        }

        private void BtnDelMaterials_Click(object sender, EventArgs e)
        {

            DelMaterials();
        }

        private void Materialtable_CellClick(object sender, TableClickEventArgs e)
        {
            // 先尝试转换为列表
            if (e.Record is UIMaterial dto)
            {
                _material = _db.Query<MaterialDto>().Where(c => c.ID == dto.ID).FirstOrDefault();
                inputName.Text = dto.Name;
                inputDescription.Text = dto.Description;
                selectType.Text = dto.Type;
                inputGrinding.Value = string.IsNullOrEmpty(dto.Grinding) ? 0 : decimal.Parse(dto.Grinding);
                inputX1.Value = string.IsNullOrEmpty(dto.X1) ? 0 : decimal.Parse(dto.X1);
                inputX2.Value = string.IsNullOrEmpty(dto.X2) ? 0 : decimal.Parse(dto.X2);
                inputY1.Value = string.IsNullOrEmpty(dto.Y1) ? 0 : decimal.Parse(dto.Y1);
                inputY2.Value = string.IsNullOrEmpty(dto.Y2) ? 0 : decimal.Parse(dto.Y2);
                inputMinimum.Value = string.IsNullOrEmpty(dto.Minimum) ? 0 : decimal.Parse(dto.Minimum);
                inputThickness.Value = string.IsNullOrEmpty(dto.Thickness) ? 0 : decimal.Parse(dto.Thickness);

                OriginalTableInfo(_material.ID);
            }
       
        }

        private void BtnMaterialSave_Click(object sender, EventArgs e)
        {

            _material.Name = inputName.Text;
            _material.Description = inputDescription.Text;
            _material.Type = selectType.Text;
            _material.Grinding = inputGrinding.Value.ToString();
            _material.X1 = inputX1.Value.ToString();
            _material.X2 = inputX2.Value.ToString();
            _material.Y1 = inputY1.Value.ToString();
            _material.Y2 = inputY2.Value.ToString();
            _material.Minimum = inputMinimum.Value.ToString();
            _material.Thickness = inputThickness.Value.ToString();

             var relust = _db.Update(_material);
            if (relust > 0 )
            {
                MaterialTbaleInfo();
            }
        }

        private void Materialtable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            var buttontext = e.Btn.Text;

            var dto = (UIMaterial)e.Record;
            switch (buttontext)
            {

                case "删除":
                    {
                        var result = AntdUI.Modal.open(this, "删除警告！", "删除后无法恢复！", TType.Warn);
                        if (result != DialogResult.OK)
                        {
                            return;
                        }
                        else
                        {
                            var count = _db.Delete<MaterialDto>(dto.ID);
                            if (count > 0)
                            {

                                MaterialTbaleInfo();
                                AntdUI.Notification.success(
                                             this,
                                             "提示",
                                             "删除成功！",
                                             autoClose: 3,
                                             align: TAlignFrom.TR
                                         );
                            }
                            else
                            {
                                AntdUI.Notification.error(
                                             this,
                                             "提示",
                                             "删除失败！",
                                             autoClose: 3,
                                             align: TAlignFrom.TR
                                         );
                            }
                        }
                        return;
                    }
            }



        }

        private void BtnAddMaterials_Click(object sender, EventArgs e)
        {
            AddMaterials();
        }
        private void AddMaterials() {

            try
            {
                var dto = new MaterialDto();
                dto.Name = UniqueCodeGenerator.Generate("MAT");
                dto.X1 = "0";
                dto.X2 = "0";
                dto.Y1 = "0";
                dto.Y2 = "0";
                dto.Minimum = "10";
                dto.Thickness = "0";
                dto.Grinding = "0";
                dto.Type = selectType.Text;
                var dtoId = _db.Insert(dto);
                MaterialTbaleInfo();
                Materialtable.SelectedIndex = -1;
                Materialtable.ScrollLine(-1);

                _material = _db.Query<MaterialDto>().Where(c => c.ID == dtoId).First();


                inputName.Text = _material.Name;
                inputDescription.Text = _material.Description;
                selectType.Text = _material.Type;
                inputGrinding.Value = string.IsNullOrEmpty(_material.Grinding) ? 0 : decimal.Parse(_material.Grinding);
                inputX1.Value = string.IsNullOrEmpty(_material.X1) ? 0 : decimal.Parse(_material.X1);
                inputX2.Value = string.IsNullOrEmpty(_material.X2) ? 0 : decimal.Parse(_material.X2);
                inputY1.Value = string.IsNullOrEmpty(_material.Y1) ? 0 : decimal.Parse(_material.Y1);
                inputY2.Value = string.IsNullOrEmpty(_material.Y2) ? 0 : decimal.Parse(_material.Y2);
                inputMinimum.Value = string.IsNullOrEmpty(_material.Minimum) ? 0 : decimal.Parse(_material.Minimum);
                inputThickness.Value = string.IsNullOrEmpty(_material.Thickness) ? 0 : decimal.Parse(_material.Thickness);


            }
            catch (Exception ex)
            {
                Serilog.Log.Warning(ex.ToString());
                throw;
            }
        }
        private void DelMaterials() {
            var Source = Materialtable.DataSource;
            if (Source is AntList<UIMaterial> dtoList)
            {
                foreach (var item in dtoList)
                {
                    if (item.Selected)
                    {
                        _db.Delete<MaterialDto>(item.ID);
                    }
                }
                MaterialTbaleInfo();
            }
        }

        private void AddOriginal() {

            try
            {
                if (_material == null || _material.ID == null)
                {
                    AntdUI.Modal.open(this, "错误提示！", "未选择正确的材料！", TType.Warn);
                    return;
                }
                var dto = new OriginalDto();
                dto.MaterialID = _material.ID;
                dto.Number = long.Parse(inputNumber.Value.ToString());
                dto.X1 = inputOX.Text;
                dto.Y1 = inputOY.Text;
                dto.Priority = inputPriority.Text;
                dto.Type = string.IsNullOrEmpty(selectOType.Text) == true ? "XY" : selectOType.Text;
                var dtoId = _db.Insert(dto);
                OriginalTableInfo(_material.ID);


                _original = _db.Query<OriginalDto>().Where(c => c.ID == dtoId).First();

                inputName.Text = _material.Name;
                inputDescription.Text = _material.Description;
                selectType.Text = _material.Type;
                inputGrinding.Value = string.IsNullOrEmpty(_material.Grinding) ? 0 : decimal.Parse(_material.Grinding);
                inputX1.Value = string.IsNullOrEmpty(_material.X1) ? 0 : decimal.Parse(_material.X1);
                inputX2.Value = string.IsNullOrEmpty(_material.X2) ? 0 : decimal.Parse(_material.X2);
                inputY1.Value = string.IsNullOrEmpty(_material.Y1) ? 0 : decimal.Parse(_material.Y1);
                inputY2.Value = string.IsNullOrEmpty(_material.Y2) ? 0 : decimal.Parse(_material.Y2);
                inputMinimum.Value = string.IsNullOrEmpty(_material.Minimum) ? 0 : decimal.Parse(_material.Minimum);
                inputThickness.Value = string.IsNullOrEmpty(_material.Thickness) ? 0 : decimal.Parse(_material.Thickness);
                OriginalTable.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Serilog.Log.Warning(ex.ToString());
                throw;
            }
        }

        private void DelOriginal() {

            var Source = OriginalTable.DataSource;
            if (Source is AntList<UIOriginal> dtoList)
            {
                foreach (var item in dtoList)
                {
                    if (item.Selected)
                    {
                        _db.Delete<OriginalDto>(item.ID);
                    }
                }
                OriginalTbaleInfo();
            }
        }
        private void Menu_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            var name = e.Value.Tag;
            switch (name)
            {
                case GlassCuttingOptimization.Enums.NavigationMenu.ExitSystem:
                    {
                        this.Close();
                    }
                    break;
                case NavigationMenu.AddMaterials:
                    {
                        AddMaterials();
                    }
                    break;
                case NavigationMenu.DelMaterials:
                    {
                        DelMaterials();
                    }
                    break;
                case NavigationMenu.AddOriginal:
                    {
                        AddOriginal();
                    }
                    break;
                case NavigationMenu.DelOriginal:
                    {
                        DelOriginal();
                    }
                    break;
                default:
                    break;
            }
        }

        private void InitData()
        {
            this.windowBar.Text = "原片库存";
            this.windowBar.DividerShow = true;
            this.windowBar.DividerThickness = 2;
            this.windowBar.ShowIcon = true;
            this.windowBar.ShowButton = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            selectType.Items.AddRange(new object[] { "夹层玻璃", "单片镀膜",
                "夹层镀膜", "丝印玻璃", "带LOW-E膜的单层玻璃", "TPF Low-E夹胶",
                "Monolithic with Film", "Monolitich EASYPRO", "Laminated EASYPRO" });
            selectType.SelectedIndex = 0;
            selectOType.Items.AddRange(new object[] { "X", "Y", "XY" });
            selectOType.SelectedIndex = 3;
        }

        private void LoadMenu(string filter = "")
        {
            menu.Items.Clear();

            foreach (var rootItem in DataOriginalUtil.MenuItems)
            {
                var rootKey = rootItem.Key.ToLower();
                var rootMenu = new AntdUI.MenuItem { Text = rootItem.Key };
                bool rootVisible = false; // 用于标记是否显示根节点


                foreach (var item in rootItem.Value)
                {
                    var childText = item.Text.ToLower();

                    // 如果子节点包含搜索文本
                    if (childText.Contains(filter))
                    {
                        var menuItem = new AntdUI.MenuItem
                        {
                            Text = item.Text,
                            IconSvg = item.IconSvg,
                            Tag = item.Tag,
                        };
                        rootMenu.Sub.Add(menuItem);
                        rootVisible = true; // 如果有子节点包含，则显示根节点
                    }
                }

                // 如果根节点包含搜索文本，或有可见的子节点，则显示根节点
                if (rootKey.Contains(filter) || rootVisible)
                {
                    menu.Items.Add(rootMenu);
                }
            }
        }

        private void InitTableColumns()
        {
            Materialtable.Columns = new ColumnCollection() {
                 new AntdUI.ColumnCheck("Selected"){Fixed = true,Width="50"},
                 new AntdUI.Column("Index", "序号", ColumnAlign.Center),
                 new AntdUI.Column("Name", "材料名称", ColumnAlign.Center),
                 new AntdUI.Column("Type", "材料类型", ColumnAlign.Center),
                 new AntdUI.Column("Description", "完整描述", ColumnAlign.Center),
                 new AntdUI.Column("Grinding", "厚度", ColumnAlign.Center),
                 new AntdUI.Column("X1", "X1", ColumnAlign.Center),
                 new AntdUI.Column("Y1", "X2", ColumnAlign.Center),
                 new AntdUI.Column("X2", "Y1", ColumnAlign.Center),
                 new AntdUI.Column("Y2", "Y2", ColumnAlign.Center),
                 new AntdUI.Column("Minimum", "最小距离", ColumnAlign.Center),
                 new AntdUI.Column("Thickness", "研磨", ColumnAlign.Center),
                 new AntdUI.Column("CellLinks", "链接"){ LineBreak = true}
              };
            Materialtable.VisibleHeader = true;
            Materialtable.AutoSizeColumnsMode = ColumnsMode.Fill;
            OriginalTable.Columns = new ColumnCollection() {
                 new AntdUI.ColumnCheck("Selected"){Fixed = true,Width="50"},
                 new AntdUI.Column("Index", "序号", ColumnAlign.Center),
                 new AntdUI.Column("Number", "数量", ColumnAlign.Center),
                 new AntdUI.Column("X1", "X尺寸", ColumnAlign.Center),
                 new AntdUI.Column("Y1", "Y尺寸", ColumnAlign.Center),
                 new AntdUI.Column("Priority", "优先级", ColumnAlign.Center),
                 new AntdUI.Column("Type", "横切类型", ColumnAlign.Center),
                 new AntdUI.Column("CellLinks", "链接"){ LineBreak = true}
              };
            OriginalTable.VisibleHeader = true;
            OriginalTable.AutoSizeColumnsMode = ColumnsMode.Fill;
        }

        public void OriginalTableInfo(long materialID)
        {
            try
            {
                var dtoList = _db.Query<OriginalDto>().Where(c=>c.MaterialID == materialID).ToList();
                var deviceList = new AntList<UIOriginal>();
                int i = 1;
                foreach (var item in dtoList)
                {
                    deviceList.Add(new UIOriginal
                    {
                        Index = i,
                        ID = item.ID,
                        MaterialID = materialID,
                        Number = item.Number,
                        X1 = item.X1,
                        Y1 = item.Y1,
                        Priority = item.Priority,
                        Type = item.Type,
                        CellLinks = new CellLink[] {
                       new CellButton(Guid.NewGuid().ToString(),"删除",TTypeMini.Error),
                    }
                    });
                    i++;
                }

                OriginaltableBinding(deviceList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void MaterialTbaleInfo()
        {
            try
            {
                var dtoList = _db.Query<MaterialDto>().ToList();
                var deviceList = new AntList<UIMaterial>();
                int i = 1;
                foreach (var item in dtoList)
                {
                    deviceList.Add(new UIMaterial
                    {
                        Index = i,
                        ID = item.ID,
                        Name = item.Name,
                        Type = item.Type,
                        Description = item.Description,
                        Grinding = item.Grinding,
                        X1 = item.X1,
                        X2 = item.X2,
                        Y1 = item.Y1,
                        Y2 = item.Y2,
                        Minimum = item.Minimum,
                        Thickness = item.Thickness,
                        CellLinks = new CellLink[] {
                       new CellButton(Guid.NewGuid().ToString(),"删除",TTypeMini.Error),
                    }
                    });
                    i++;
                }

                tableBinding(deviceList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void OriginalTbaleInfo()
        {
            try
            {
                var dtoList = _db.Query<OriginalDto>().Where(c=>c.MaterialID == _material.ID).ToList();
                var deviceList = new AntList<UIOriginal>();
                int i = 1;
                foreach (var item in dtoList)
                {
                    deviceList.Add(new UIOriginal
                    {
                        Index = i,
                        ID = item.ID,
                        MaterialID = item.MaterialID,
                        Number = item.Number,
                        X1 = item.X1,
                        Y1 = item.Y1,
                        Priority = item.Priority,
                        Type = item.Type,
                        CellLinks = new CellLink[] {
                       new CellButton(Guid.NewGuid().ToString(),"删除",TTypeMini.Error),
                    }
                    });
                    i++;
                }

                OriginaltableBinding(deviceList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private async void OriginaltableBinding(AntList<UIOriginal> dtoList)
        {
            OriginalTable.DataSource = null;
            OriginalTable.Binding(dtoList);
        }

        private async void tableBinding(AntList<UIMaterial> dtoList)
        {
            Materialtable.DataSource = null;
            Materialtable.Binding(dtoList);
        }
    }
}
