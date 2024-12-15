using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public string IC { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
