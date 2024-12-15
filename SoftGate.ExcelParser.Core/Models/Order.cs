using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Models
{
    public class Order
    {
        public string Number { get; set; }
        public List<OrderPeriodAmount> MonthlyAmount { get; set; } = new List<OrderPeriodAmount>();
    }
}
