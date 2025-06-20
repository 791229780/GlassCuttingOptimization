using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.UIModels
{
    public class UIOriginal : NotifyProperty
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
        /// 材料外键
        /// </summary>
        public long materialID { get; set; }

        public long MaterialID
        {
            get { return materialID; }
            set
            {
                if (materialID == value) return;
                materialID = value;
                OnPropertyChanged(nameof(MaterialID));
            }
        }
        /// <summary>
        /// 数量
        /// </summary>
        public long number { get; set; }
        public long Number
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
        /// X尺寸
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
        /// Y尺寸
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
        /// 优先级
        /// </summary>
        public string priority { get; set; }
        public string Priority
        {
            get { return priority; }
            set
            {
                if (priority == value) return;
                priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }
        /// <summary>
        /// 横切类型
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
    }
}
