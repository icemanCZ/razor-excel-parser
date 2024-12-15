using Microsoft.Extensions.DependencyInjection;
using SoftGate.ExcelParser.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftGate.ExcelParser.Core.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection UseExcelParser(this IServiceCollection services)
        {
            services.AddSingleton<ExcelParserService>();
            services.AddSingleton<DataAgregatorService>();
            return services;
        }
    }
}
