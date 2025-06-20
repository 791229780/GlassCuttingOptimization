using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Code
{
    public class ComboboxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        // 重写 ToString 方法，这样 ComboBox 显示的是 Text
        public override string ToString()
        {
            return Text;
        }
    }
}
