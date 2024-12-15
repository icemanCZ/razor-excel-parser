using SoftGate.ExcelParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Services
{
    public class DataAgregatorService
    {
        /// <summary>
        /// summarize excel values by customers, sums all duplicit order numbers and periods
        /// </summary>
        public IEnumerable<Customer> AgregateByCustomer(IEnumerable<ExcelRow> rows)
        {
            foreach (var customerRows in rows.GroupBy(row => row.IC))
            {
                var customer = new Customer();
                customer.Name = customerRows.First().CustomerName;
                customer.IC = customerRows.Key;
                foreach (var customerOrderRows in customerRows.GroupBy(x => x.OrderNumber))
                {
                    var customerOrder = new Order();
                    customerOrder.Number = customerOrderRows.Key;
                    customerOrder.MonthlyAmount = customerOrderRows.SelectMany(x => x.PeriodAmounts)
                        .GroupBy(x => x.Period)
                        .Select(x => new OrderPeriodAmount(x.Key, x.Sum(a => a.Amount)))
                        .ToList();
                    customer.Orders.Add(customerOrder);
                }
                yield return customer;
            }
        }
    }
}
