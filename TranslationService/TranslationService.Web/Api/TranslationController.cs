using PersistenceLayer.ValueObjects;
using PersistenceLayer.ViewModel.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TranslationService.Services;
using TranslationService.Validation;

namespace TranslationService.Web.Api
{
    [RoutePrefix("api/translation/{id}")]
    public class TranslationController : ApiController
    {
        private TranslationService.Services.TranslationService _translationService;
        private TranslationValidator _translationValidator;

        public TranslationController()
        {
            _translationService = new TranslationService.Services.TranslationService();
            _translationValidator = new TranslationValidator();
        }

        [HttpGet]
        public async Task<List<TranslationVO>> Get(int id)
        {
            var result = await _translationService.GetTranslationsForCulture(id);
            return result;
        }

        [HttpPost]
        public void Post([FromBody]TranslationDTO translations)
        {
            var validationResult = _translationValidator.Validate(translations);
            if(validationResult.IsFailed)
            {
                throw new Exception(validationResult.Error);
            }
            _translationService.SaveTranslationForCulture(translations.CultureId, translations.Translations);
        }
    }    
}
