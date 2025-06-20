using GlassCuttingOptimization.Models.Model;
using GlassCuttingOptimization.Models.UIItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Dto
{
   public  class OrderDto
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }

        public ComboboxItem Materials { get; set; }
        /// <summary>
        /// X尺寸
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y尺寸
        /// </summary>
        public int Y { get; set; }

          /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string Customer { get; set; }
        /// <summary>
        /// 订单
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public string SendDate { get; set; }

        /// <summary>
        /// 是否旋转
        /// </summary>

        public string Rotation { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Rriority { get; set; }
        /// <summary>
        /// 注释模型
        /// </summary>
        public NoteModel modelNote { get; set; }

       /// <summary>
       /// 底部
       /// </summary>
        public int BottomNumber { get; set;   }
        
        /// <summary>
        /// 顶部
        /// </summary>
        public int TopNumber { get; set; }

        /// <summary>
        /// 右部
        /// </summary>
        public int RightNumber { get; set; }

        /// <summary>
        /// 左部
        /// </summary>
        public int LeftNumber { get; set; }

        /// <summary>
        /// 底部
        /// </summary>
        public bool BottomCbo { get; set; }

        /// <summary>
        /// 顶部
        /// </summary>
        public bool TopCbo { get; set; }

        /// <summary>
        /// 右部
        /// </summary>
        public bool RightCbo { get; set; }

        /// <summary>
        /// 左部
        /// </summary>
        public bool LeftCbo { get; set; }

    }
}
