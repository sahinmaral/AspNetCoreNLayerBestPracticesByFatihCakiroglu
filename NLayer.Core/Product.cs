using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core
{
    public class Product : BaseEntity
    {
        // Class icerisindeki field ve property lerin erisim belirleyicisi private
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public ProductFeature ProductFeature { get; set; }
    }
}
