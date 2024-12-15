using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Models
{
    public class ExcelRow
    {
        public class PeriodAmount
        {
            public DateTime Period { get; set; }
            public int Amount { get; set; }

            public PeriodAmount(DateTime period, int amount)
            {
                Period = period;
                Amount = amount;
            }
        }

        public string CustomerName { get; set; }
        public string IC { get; set; }
        public string OrderNumber { get; set; }
        public List<PeriodAmount> PeriodAmounts { get; set; } = new List<PeriodAmount>();

        public ExcelRow(string customerName, string iC, string orderNumber)
        {
            CustomerName = customerName;
            IC = iC;
            OrderNumber = orderNumber;
        }
    }
}
