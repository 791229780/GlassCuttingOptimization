using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.UIModels
{
    public class UIMaterial : NotifyProperty
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
        /// 主键
        /// </summary>
        private long id { get; set; }
        public long ID
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private CellLink[] cellLinks;
        public CellLink[] CellLinks
        {
            get { return cellLinks; }
            set
            {
                if (cellLinks == value) return;
                cellLinks = value;
                OnPropertyChanged(nameof(CellLinks));
            }
        }



        /// <summary>
        /// 材料名称
        /// </summary>
        public string name { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        /// <summary>
        /// 材料类型
        /// </summary>
        public string type { get; set; }
        public string Type
        {
            get { return type; }
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        /// <summary>
        /// 完整描述
        /// </summary>
        public string description { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                if (description == value) return;
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        /// <summary>
        /// 厚度
        /// </summary>
        public string grinding { get; set; }

        public string Grinding
        {
            get { return grinding; }
            set
            {
                if (grinding == value) return;
                grinding = value;
                OnPropertyChanged(nameof(Grinding));
            }
        }
        /// <summary>
        /// X1
        /// </summary>
        public string x1 { get; set; }
        public string X1
        {
            get { return x1; }
            set
            {
                if (x1 == value) return;
                x1 = value;
                OnPropertyChanged(nameof(X1));
            }
        }

        /// <summary>
        /// X2
        /// </summary>
        public string x2 { get; set; }
        public string X2
        {
            get { return x2; }
            set
            {
                if (x2 == value) return;
                x2 = value;
                OnPropertyChanged(nameof(X2));
            }
        }

        /// <summary>
        /// Y1
        /// </summary>
        public string y1 { get; set; }
        public string Y1
        {
            get { return y1; }
            set
            {
                if (y1 == value) return;
                y1 = value;
                OnPropertyChanged(nameof(Y1));
            }
        }
        /// <summary>
        /// Y2
        /// </summary>
        public string y2 { get; set; }
        public string Y2
        {
            get { return y2; }
            set
            {
                if (y2 == value) return;
                y2 = value;
                OnPropertyChanged(nameof(Y2));
            }
        }
        /// <summary>
        /// 最小距离
        /// </summary>
        public string minimum { get; set; }
        public string Minimum
        {
            get { return minimum; }
            set
            {
                if (minimum == value) return;
                minimum = value;
                OnPropertyChanged(nameof(Minimum));
            }
        }
        /// <summary>
        /// 研磨
        /// </summary>
        public string thickness { get; set; }
        public string Thickness
        {
            get { return thickness; }
            set
            {
                if (thickness == value) return;
                thickness = value;
                OnPropertyChanged(nameof(Thickness));
            }
        }
    }
}
