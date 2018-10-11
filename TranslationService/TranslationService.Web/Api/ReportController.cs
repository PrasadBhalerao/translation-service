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
using System.Web.Http;
using System.Web.Mvc;

namespace TranslationService.Web.Api
{
    [System.Web.Http.RoutePrefix("api/report/{id}")]
    public class ReportController : ApiController
    {
        private ExcelService _excelService;
        public ReportController()
        {
            _excelService = new ExcelService();
        }

        [System.Web.Http.HttpGet]
        public async Task<ExcelReportVO> Get(int id)
        {
            var fileName = await _excelService.GenerateReport(id);
            var path = @"\Content\Excel\" + fileName;
            return new ExcelReportVO()
            {
                Path = path
            };
        }
    }
}
