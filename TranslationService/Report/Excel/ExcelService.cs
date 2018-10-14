using Newtonsoft.Json;
using OfficeOpenXml;
using PersistenceLayer.ValueObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using TranslationService.Services;

namespace Report.Excel
{
    public class ExcelService
    {
        private TranslationService.Services.TranslationService _translationService;
        private CultureService _cultureService;
        private string _templatePath;


        public ExcelService()
        {
            _translationService = new TranslationService.Services.TranslationService();
            _cultureService = new CultureService();
            _templatePath = HostingEnvironment.MapPath("~/Content/ExportFiles/");
        }

        public async Task<string> GenerateExcelReport(int cultureId)
        {
            var translations = await _translationService.GetTranslationsForCulture(cultureId);
            var cultureInfo = await _cultureService.GetCultureInfo(cultureId);

            var excelPackage = new ExcelPackage();
            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets.Add(cultureInfo.CultureName);

            //setting headers
            var cell = sheet.Cells[1, 1];
            cell.Value = "Translation key";
            cell.Style.Font.Bold = true;
            cell = sheet.Cells[1, 2];
            cell.Value = "Translation value";
            cell.Style.Font.Bold = true;

            //setting translation pairs
            var rowId = 2;
            foreach (var translation in translations)
            {
                cell = sheet.Cells[rowId, 1];
                cell.Value = translation.Key;
                cell = sheet.Cells[rowId, 2];
                cell.Value = translation.Value;
                rowId++;
            }

            sheet.Protection.IsProtected = false;
            sheet.Protection.AllowSelectLockedCells = false;

            string fileName, path;
            fileName = GetFileName(cultureInfo.DisplayName, ".xlsx");
            path = Path.Combine(_templatePath, fileName);

            var fileInfo = new FileInfo(path);
            excelPackage.SaveAs(fileInfo);
            return fileName;
        }

        private string GetFileName(string cultureName, string extension)
        {
            return cultureName + " on " + DateTime.Now.ToString().Replace(':', '-') + extension;
        }

        public async Task<string> GenerateJSONReport(int cultureId)
        {
            var translations = await _translationService.GetTranslationsForCulture(cultureId);
            var cultureInfo = await _cultureService.GetCultureInfo(cultureId);
            var keyValuePairs = translations.Select(x => new { x.Key, x.Value });

            string json = JsonConvert.SerializeObject(keyValuePairs.ToArray());

            string fileName, path;
            fileName = GetFileName(cultureInfo.DisplayName, ".txt");
            path = Path.Combine(_templatePath, fileName);
            System.IO.File.WriteAllText(path, json);

            return fileName;
        }

        public async Task SaveExcelData(int cultureId, Stream file)
        {
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                int totalRows = workSheet.Dimension.Rows;

                var translations = new List<TranslationVO>();
                for (int i = 2; i <= totalRows; i++)
                {
                    translations.Add(new TranslationVO
                    {
                        Key = workSheet.Cells[i, 1].Value.ToString(),
                        Value = workSheet.Cells[i, 2].Value.ToString(),
                    });
                }

                await _translationService.SaveTranslationForCulture(cultureId, translations);
            }
            return;
        }
    }
}
