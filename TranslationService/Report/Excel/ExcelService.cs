using OfficeOpenXml;
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

        public ExcelService()
        {
            _translationService = new TranslationService.Services.TranslationService();
            _cultureService = new CultureService();
        }

        public async Task<string> GenerateReport(int cultureId)
        {
            var translations = await _translationService.GetTranslationsForCulture(cultureId);
            var cultureInfo = await _cultureService.GetCultureInfo(cultureId);

            var excelPackage = new ExcelPackage();
            ExcelWorksheet sheet= excelPackage.Workbook.Worksheets.Add(cultureInfo.CultureName);

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

            var templatePath = HostingEnvironment.MapPath("~/Content/Excel/");
            var fileName = cultureInfo.DisplayName + " on " + DateTime.Now.ToString().Replace(':', '-') + ".xlsx";
            var path = Path.Combine(templatePath, fileName);
            var fileInfo = new FileInfo(path);
            excelPackage.SaveAs(fileInfo);

            return fileName;
        }
    }
}
