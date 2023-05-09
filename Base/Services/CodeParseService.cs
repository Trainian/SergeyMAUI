using Base.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services
{
    public class CodeParseService : ICodeParseService
    {
        private readonly ILogger<CodeParseService> _logger;
        private const string _site = "https://barcode-list.ru/";
        private const string _barcode = "barcode/RU/%D0%9F%D0%BE%D0%B8%D1%81%D0%BA.htm?barcode=";

        public CodeParseService(ILogger<CodeParseService> logger)
        {
            _logger = logger;
        }
        public async Task<string> FindNameByCode(string code)
        {
            string result = "";
            HttpClient client = new HttpClient();
            try
            {
                var request = await client.GetAsync(_site + _barcode + code);
                result = await request.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return "NULL";
            }

            return "***" + ParsElement(result);
        }

        private string ParsElement(string element)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(element);

            var result = html.DocumentNode.Descendants("h1")
                .Where(s => s.GetAttributeValue("class", "pageTitle") != "")
                .ToList();
            var strings = result.First().FirstChild.InnerText.Split(" - Штрих-код:");
            return strings[0].StartsWith("Поиск:") ? "NULL" : strings[0] ?? "NULL";
        }
    }
}
