
using Microsoft.Extensions.DependencyInjection;
using SoftGate.ExcelParser.Core.Extensions;
using SoftGate.ExcelParser.Core.Services;
using System.Text.Json;

namespace SoftGate.ExcelParser.Console
{
    public class Program
    {
        private readonly ExcelParserService _excelParserService;
        private readonly DataAgregatorService _dataAgregatorService;

        public Program(ExcelParserService excelParserService, DataAgregatorService dataAgregatorService)
        {
            _excelParserService = excelParserService ?? throw new ArgumentNullException(nameof(excelParserService));
            _dataAgregatorService = dataAgregatorService ?? throw new ArgumentNullException(nameof(dataAgregatorService));
        }

        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<Program>()
                .UseExcelParser()
                .BuildServiceProvider();

            serviceProvider.GetRequiredService<Program>().Run(args);
        }

        private void Run(string[] args)
        {
            System.Console.WriteLine("Vyhledávám soubory...");
            var fileNames = Directory.GetFiles(".", "*.xlsx");
            if (fileNames.Any())
            {
                System.Console.WriteLine($"Nalezeno {fileNames.Count()} souborů");
                foreach (var fileName in fileNames)
                {
                    try
                    {
                        ProcessFile(fileName);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Chyba zpracování souboru {fileName}: {ex.Message}");
                    }
                }
            }
            else
            {
                System.Console.WriteLine($"Nebyly nalezeny žádné soubory ve formátu xlsx.");
            }
        }

        private void ProcessFile(string fileName)
        {
            System.Console.WriteLine($"Zpracovávám soubor {fileName}...");
            var excelRows = _excelParserService.Parse(File.ReadAllBytes(fileName));
            var aggregatedData = _dataAgregatorService.AgregateByCustomer(excelRows).ToList();
            var newFileName = fileName.Substring(0, fileName.LastIndexOf('.')) + ".json";
            if (File.Exists(newFileName))
            {
                do
                {
                    System.Console.WriteLine($"Soubor {newFileName} už existuje. Přepsat? A/N");
                    var overwrite = System.Console.ReadLine()?.ToLower();
                    if (overwrite == "a")
                        break;
                    if (overwrite == "n")
                        return;
                } while (true);
            }
            File.WriteAllText(newFileName, JsonSerializer.Serialize(aggregatedData));
        }
    }
}
