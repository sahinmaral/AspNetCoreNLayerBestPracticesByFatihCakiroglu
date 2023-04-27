using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class ProductFeature
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
