using PersistenceLayer.ViewModel.cs;
using Report.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using TranslationService.Validation;

namespace TranslationService.Web.Api
{
    [System.Web.Http.RoutePrefix("api/report/{id}")]
    public class ReportController : ApiController
    {
        private ExcelService _excelService;
        private ReportValidator _reportValidator;

        public ReportController()
        {
            _excelService = new ExcelService();
            _reportValidator = new ReportValidator();
        }

        [System.Web.Http.HttpGet]
        public async Task<ExcelReportVO> Get(int id, bool isJson)
        {
            var fileName = "";
            if (isJson)
            {
                fileName = await _excelService.GenerateJSONReport(id);
            }
            else
            {
                fileName = await _excelService.GenerateExcelReport(id);
            }
            var path = @"\Content\ExportFiles\" + fileName;
            return new ExcelReportVO()
            {
                Path = path
            };
        }

        [System.Web.Http.HttpPost]
        public async Task UploadFile([FromUri]int id)
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files[0] : null;            

            var validationResult = _reportValidator.Validate(file);
            if (validationResult.IsFailed)
            {
                throw new Exception(validationResult.Error);
            }

            await _excelService.SaveExcelData(id, file.InputStream);
        }

    }
}
