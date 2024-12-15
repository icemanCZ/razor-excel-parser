using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using SoftGate.ExcelParser.Core.Services;
using System.Text.Json;
using System.Text;
using System.Security.Cryptography;

namespace SoftGate.ExcelParser.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ExcelParserService _excelParserService;
        private readonly DataAgregatorService _dataAgregatorService;

        [BindProperty]
        public IFormFile Upload { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ExcelParserService excelParserService, DataAgregatorService dataAgregatorService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _excelParserService = excelParserService ?? throw new ArgumentNullException(nameof(excelParserService));
            _dataAgregatorService = dataAgregatorService ?? throw new ArgumentNullException(nameof(dataAgregatorService));
        }

        public void OnGet()
        {
            ViewData["ConvertError"] = TempData["ConvertError"];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Upload?.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        await Upload.CopyToAsync(ms);
                        var excelRows = _excelParserService.Parse(ms.ToArray());
                        var aggregatedData = _dataAgregatorService.AgregateByCustomer(excelRows).ToList();
                        var _fileBytes = Encoding.Default.GetBytes(JsonSerializer.Serialize(aggregatedData));
                        var _fileName = Upload.FileName.Substring(0, Upload.FileName.LastIndexOf('.')) + ".json";
                        return File(_fileBytes, "application/json", _fileName);
                    }
                }
                catch (Exception ex)
                {
                    TempData["ConvertError"] = "Chyba pøevodu: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ConvertError"] = "Prosím vyberte soubor";
                return RedirectToAction("Index");
            }

        }
    }
}
