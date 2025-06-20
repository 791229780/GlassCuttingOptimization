using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassCuttingOptimization.Models;
using GlassCuttingOptimization.Utils;
using GlassCuttingOptimization.Models.UIModels;
using Newtonsoft.Json;
using System.IO;
using GlassCuttingOptimization.Models.Code;
using AntdUI;
using NAudio.CoreAudioApi;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GlassCuttingOptimization.Views.CustomerView
{
    public partial class CustomerEditControl: UserControl
    {
        public event EventHandler<Tuple<bool>> OnSuccess_Event;
        
        private RegionModel _rootData;

        public bool _edit;
        private Dictionary<string, RegionComboBoxGroup> _comboBoxGroups;
        AntdUI.Window _windows;
        UICustomerInfoDto _device;
        SqlSugarHelper _db;
        public CustomerEditControl(AntdUI.Window window, UICustomerInfoDto device, SqlSugarHelper db, bool edit = false)
        {
            _edit = edit;
            _windows = window;
            _device = device;
            _db = db;
            //panel3.ShadowOpacity = (float)0.3;
            //panel3.Shadow = 2;
            InitializeComponent();
            InitData();
            InitializeControls();

            LoadProvinces();
            BindEventHandler();
            if (!edit)
            {
                InitData();
            }
            else
            {
                EditInitData();
            }
        }

        private void EditInitData()
        {
         
            inputcustomer_code.Text = _device.customer_code;
            inputcompany_name.Text = _device.company_name;

            inputcontact_person.Text = _device.contact_person;
            selectCustomer_type.Text = _device.customer_type;
            inputphone.Text = _device.phone;
            inputmobile.Text = _device.mobile;
            inputemail.Text = _device.email;
            selectProvince.Text = _device.province;
            selectCity.Text = _device.city;

            selectDistrict.Text = _device.district;
            inputaddress.Text = _device.address;
            inputremarks.Text = _device.remarks;
        }
        private void BindEventHandler()
        {
            // 初始化组合框组
            _comboBoxGroups = new Dictionary<string, RegionComboBoxGroup>
             {
                 { "NativePlace", new RegionComboBoxGroup(selectProvince, selectCity, selectDistrict) },
           
             };

            // 注册事件处理程序
            foreach (var group in _comboBoxGroups.Values)
            {
                group.Province.SelectedIndexChanged += (s, e) => HandleProvinceSelection(s as Select);
                group.City.SelectedIndexChanged += (s, e) => HandleCitySelection(s as Select);
            }

            btnSubmit.Click += BtnSubmit_Click;
            btnClose.Click += BtnClose_Click;
        }
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputcustomer_code.Text) || string.IsNullOrEmpty(inputcompany_name.Text))
            {

                MessageBox.Show("数据不可为空！");
                return;
            }

            if (_edit)
            {
                var dto = _db.Query<CustomerInfoDto>().Where(c => c.customer_id == _device.ID).First();


                dto.customer_code = inputcustomer_code.Text;
                dto.company_name = inputcompany_name.Text;
                dto.contact_person = inputcontact_person.Text;
                dto.customer_type = selectCustomer_type.Text;
                dto.phone = inputphone.Text;
                dto.mobile = inputmobile.Text;
                dto.email = inputemail.Text;

                dto.province = selectProvince.Text;
                dto.city = selectCity.Text;
                dto.district = selectDistrict.Text;

                dto.address = inputaddress.Text;
                dto.remarks = inputremarks.Text;
                _db.Update(dto);
            }
            else
            {
                var dto = new CustomerInfoDto();
                dto.customer_code = inputcustomer_code.Text;
                dto.company_name = inputcompany_name.Text;
                dto.contact_person = inputcontact_person.Text;
                dto.customer_type = selectCustomer_type.Text;
                dto.phone = inputphone.Text;
                dto.mobile = inputmobile.Text;
                dto.email = inputemail.Text;

                dto.province = selectProvince.Text;
                dto.city = selectCity.Text;
                dto.district = selectDistrict.Text;

                dto.address = inputaddress.Text;
                dto.remarks = inputremarks.Text;
                _db.Insert(dto);
            }
            OnSuccess_Event?.Invoke(OnSuccess_Event, new Tuple<bool>(true));
            this.Dispose();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void HandleCitySelection(Select cityComboBox)
        {
            // 找到对应的组合框组
            var group = _comboBoxGroups.Values.FirstOrDefault(g => g.City == cityComboBox);
            if (group == null) return;

            // 清空区县
            ClearComboBoxes(group.District);

            if (group.Province.SelectedValue == null || cityComboBox.SelectedValue == null)
            {
                return;
            }
            var selectedProvinceItem = (ComboboxItem)group.Province.SelectedValue;
            var selectedCityItem = (ComboboxItem)cityComboBox.SelectedValue;
            //if (selectedProvinceItem == null || selectedCityItem == null) return;

            var selectedProvince = _rootData.List.FirstOrDefault(p => p.Code.ToString() == selectedProvinceItem.Value);
            if (selectedProvince == null) return;

            var selectedCity = selectedProvince.List.FirstOrDefault(c => c.Code.ToString() == selectedCityItem.Value);
            if (selectedCity == null) return;

            // 填充区县数据
            FillComboBox(group.District, selectedCity.List.Select(district => new ComboboxItem
            {
                Text = district.Name,
                Value = district.Code.ToString()
            }));
        }

        private void HandleProvinceSelection(Select provinceComboBox)
        {
            // 找到对应的组合框组
            var group = _comboBoxGroups.Values.FirstOrDefault(g => g.Province == provinceComboBox);
            if (group == null) return;

            // 清空下级选项
            ClearComboBoxes(group.City, group.District);
            if (provinceComboBox.SelectedValue == null)
            {
                return;
            }
            var selectedItem = (ComboboxItem)provinceComboBox.SelectedValue;
            //if (selectedItem == null) return;

            var selectedProvince = _rootData.List.FirstOrDefault(p => p.Code.ToString() == selectedItem.Value);
            if (selectedProvince == null) return;
            // 获取城市列表
            var cities = selectedProvince.List.ToList();

            // 如果选中的是山东省，将青岛市置于首位
            if (selectedProvince.Name == "山东省")
            {
                var qingdao = cities.FirstOrDefault(c => c.Name == "青岛市");
                if (qingdao != null)
                {
                    cities.Remove(qingdao);
                    cities.Insert(0, qingdao);
                }
            }

            // 填充城市数据
            FillComboBox(group.City, cities.Select(city => new ComboboxItem
            {
                Text = city.Name,
                Value = city.Code.ToString()
            }));
        }

        private void ClearComboBoxes(params Select[] comboBoxes)
        {
            foreach (var comboBox in comboBoxes)
            {
                comboBox.Items.Clear();
                comboBox.Clear();
            }
        }

        private void FillComboBox(Select comboBox, IEnumerable<ComboboxItem> items)
        {
            foreach (var item in items)
            {
                comboBox.Items.Add(item);
            }
        }


        private void InitData()
        {
            selectCustomer_type.SelectedIndex = 0;

        }

        private void LoadProvinces()
        {

            selectProvince.Items.Clear();
            selectCity.Items.Clear();
            selectDistrict.Items.Clear();

            var provinces = _rootData.List.ToList();
            // 查找山东省
            var shandong = provinces.FirstOrDefault(p => p.Name == "山东省");
            if (shandong != null)
            {
                // 从原列表中移除山东省
                provinces.Remove(shandong);
                // 将山东省插入到列表开头
                provinces.Insert(0, shandong);
            }
            foreach (var province in provinces)
            {

                selectProvince.Items.Add(new ComboboxItem
                {
                    Text = province.Name,
                    Value = province.Code.ToString()
                });

           

            }
        }

        private void InitializeControls()
        {
            string jsonFilePath = "administrative.json";
            using (StreamReader sr = new StreamReader(jsonFilePath))
            {
                string json = sr.ReadToEnd();

                _rootData = JsonConvert.DeserializeObject<RegionModel>(json);

            }
        }
    }
}
