using ClosedXML.Excel;
using SoftGate.ExcelParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Services
{
    public class ExcelParserService
    {
        /// <summary>
        /// parse input excel bytes into ExcelRowData
        /// </summary>
        public IEnumerable<ExcelRow> Parse(byte[] data)
        {
            var ret = new List<ExcelRow>();
            using (var ms = new MemoryStream(data))
            using (var wb = new XLWorkbook(ms))
            {
                foreach (var ws in wb.Worksheets)
                {
                    // detect columns with period amounts
                    var periodsByCell = new Dictionary<int, DateTime>();
                    var periodColumn = 4;
                    while (ws.Row(1).Cell(periodColumn).TryGetValue<DateTime>(out var period))
                        periodsByCell.Add(periodColumn++, period);

                    // parse data rows untill IC column is empty
                    var rowNum = 2;
                    do
                    {
                        var row = ws.Row(rowNum);
                        if (string.IsNullOrEmpty(row.Cell(2).GetValue<string>()))
                            break;  // IC value is empty

                        var rowData = new ExcelRow(row.Cell(1).GetValue<string>(), row.Cell(2).GetValue<string>(), row.Cell(3).GetValue<string>()); // customer name, ic, order number
                        foreach (var periodCellIndex in periodsByCell.Keys)
                        {
                            if (row.Cell(periodCellIndex).TryGetValue<int>(out var amount))
                                rowData.PeriodAmounts.Add(new ExcelRow.PeriodAmount(periodsByCell[periodCellIndex], amount));
                        }
                        ret.Add(rowData);
                        rowNum++;
                    }
                    while (true);
                }
            }
            return ret;
        }
    }
}
