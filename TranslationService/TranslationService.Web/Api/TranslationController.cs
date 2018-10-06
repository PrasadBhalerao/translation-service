using PersistenceLayer.ValueObjects;
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
    [RoutePrefix("api/translation/{id}")]
    public class TranslationController : ApiController
    {
        private TranslationService.Services.TranslationService _translationService;

        public TranslationController()
        {
            _translationService = new TranslationService.Services.TranslationService();
        }

        [HttpGet]
        public async Task<List<TranslationVO>> Get(int id)
        {
            var result = await _translationService.GetTranslationsForCulture(id);
            return result;
        }
    }
}
