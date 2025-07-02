using System.Drawing;

namespace GlassCuttingOptimization.Models.Dto
{
    /// <summary>
    /// 表示已排版的玻璃片（带位置、旋转、序号、订单号等信息）
    /// </summary>
    public class PlacedGlassDto
    {
        /// <summary>
        /// 订单信息
        /// </summary>
        public OrderDto Order { get; set; }

        /// <summary>
        /// 左上角X坐标（板材内，单位mm）
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 左上角Y坐标（板材内，单位mm）
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 是否旋转90度
        /// </summary>
        public bool Rotated { get; set; }

        /// <summary>
        /// 优化序号（第几块）
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 获取实际宽度
        /// </summary>
        public int Width => Rotated ? Order.Y : Order.X;
        /// <summary>
        /// 获取实际高度
        /// </summary>
        public int Height => Rotated ? Order.X : Order.Y;

        /// <summary>
        /// 获取玻璃片矩形区域
        /// </summary>
        public Rectangle GetRect()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}