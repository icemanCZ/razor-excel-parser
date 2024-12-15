using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Models
{
    public class OrderPeriodAmount
    {
        public DateTime Period { get; set; }
        public int Amount { get; set; }

        public OrderPeriodAmount(DateTime period, int amount)
        {
            Period = period;
            Amount = amount;
        }
    }
}
