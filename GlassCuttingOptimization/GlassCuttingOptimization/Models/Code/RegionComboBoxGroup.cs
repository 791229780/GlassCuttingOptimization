using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Code
{
    public class RegionComboBoxGroup
    {
        public Select Province { get; set; }
        public Select City { get; set; }
        public Select District { get; set; }

        public RegionComboBoxGroup(Select province, Select city, Select district)
        {
            Province = province;
            City = city;
            District = district;
        }
    }
}
