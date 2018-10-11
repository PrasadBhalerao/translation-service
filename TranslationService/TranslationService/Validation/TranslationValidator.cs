using PersistenceLayer;
using PersistenceLayer.ViewModel.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationService.Validation
{
    public class TranslationValidator
    {
        private DataContext _db;
        public TranslationValidator()
        {
            _db = new DataContext();
        }

        public ValidtionResult Validate(TranslationDTO translations)
        {
            var validationResult = new ValidtionResult()
            {
                IsFailed = true,
                Errors = new List<string>()
                {
                    "1. Duplicate keys - 'NewText'",
                    "2. Duplicate keys - 'OldText'",
                },
                Error = "1. Duplicate keys - 'NewText'\n2. Duplicate keys - 'OldText'"
            };
            return validationResult;
        }
    }

    public class ValidtionResult
    {
        public bool IsFailed { get; set; }
        public List<string> Errors { get; set; }
        public string Error { get; set; }

    }
}
