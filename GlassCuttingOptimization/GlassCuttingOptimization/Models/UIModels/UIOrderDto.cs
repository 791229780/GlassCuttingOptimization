using AntdUI;
using GlassCuttingOptimization.Models.Dto;
using GlassCuttingOptimization.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GlassCuttingOptimization.Models.UIModels
{
    public class UIOrderDto : NotifyProperty
    {

        /// <summary>
        /// 选择
        /// </summary>
        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected == value) return;
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }



        /// <summary>
        /// 序号
        /// </summary>
        private long index { get; set; }
        public long Index
        {
            get { return index; }
            set
            {
                if (index == value) return;
                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        /// <summary>
        /// 材料
        /// </summary>
        public MaterialDto materials { get; set; }

        public MaterialDto MaterialsX
        {
            get { return materials; }
            set
            {
                if (materials == value) return;
                materials = value;
                OnPropertyChanged(nameof(MaterialsX));
            }
        }

        /// <summary>
        /// X尺寸
        /// </summary>
        public int x { get; set; }

        public int X
        {
            get { return x; }
            set
            {
                if (x == value) return;
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }
        /// <summary>
        /// Y尺寸
        /// </summary>
        public int y { get; set; }


        public int Y
        {
            get { return y; }
            set
            {
                if (y == value) return;
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
  
        /// <summary>
        /// 客户
        /// </summary>
        public string customer { get; set; }
        public string Customer
        {
            get { return customer; }
            set
            {
                if (customer == value) return;
                customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }
        /// <summary>
        /// 订单
        /// </summary>
        public string order { get; set; }

        public string Order
        {
            get { return order; }
            set
            {
                if (order == value) return;
                order = value;
                OnPropertyChanged(nameof(Order));
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string note { get; set; }
        public string Note
        {
            get { return note; }
            set
            {
                if (note == value) return;
                note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int riority { get; set; }
        public int Riority
        {
            get { return riority; }
            set
            {
                if (riority == value) return;
                riority = value;
                OnPropertyChanged(nameof(Riority));
            }
        }
        /// <summary>
        /// 注释模型
        /// </summary>
        public NoteModel modelNote { get; set; }

        public NoteModel ModelNote
        {
            get { return modelNote; }
            set
            {
                if (modelNote == value) return;
                modelNote = value;
                OnPropertyChanged(nameof(ModelNote));
            }
        }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int orderID { get; set; }

        public int OrderID
        {
            get { return orderID; }
            set
            {
                if (orderID == value) return;
                orderID = value;
                OnPropertyChanged(nameof(OrderID));
            }
        }


        /// <summary>
        /// 数量
        /// </summary>
        public int number { get; set; }

        public int Number
        {
            get { return number; }
            set
            {
                if (number == value) return;
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }


        /// <summary>
        /// 发货日期
        /// </summary>
        public string sendDate { get; set; }

        public string SendDate 
        {
            get { return sendDate; }
            set
            {
                if (sendDate == value) return;
                sendDate = value;
                OnPropertyChanged(nameof(SendDate));
            }
        }
        /// <summary>
        /// 是否旋转
        /// </summary>

        public string rotation { get; set; }

        public string Rotation
        {
            get { return rotation; }
            set
            {
                if (rotation == value) return;
                rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }
    }
}
