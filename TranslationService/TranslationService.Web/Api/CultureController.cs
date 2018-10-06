using PersistenceLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TranslationService.Services;

namespace TranslationService.Web.Api
{
    [RoutePrefix("api/culture/")]
    public class CultureController : ApiController
    {
        private CultureService _cultureService;
        public CultureController()
        {
            _cultureService = new CultureService();
        }

        [HttpGet]
        public async Task<List<Culture>> Get()
        {
            string searchString = "";
            var result = await _cultureService.GetCultures(searchString);
            return result;
        }
    }
}
