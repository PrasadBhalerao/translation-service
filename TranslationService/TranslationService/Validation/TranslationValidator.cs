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

        public ValidationResult Validate(TranslationDTO translations)
        {
            var error = "";

            //duplicate keys
            var duplicateItems = translations.Translations.GroupBy(x => x.Key).Where(y => y.Count() > 1).SelectMany(z => z);

            var duplicateKeys = duplicateItems.Select(x => x.Key).Distinct().ToList();

            var count = 1;
            duplicateKeys.ForEach(x =>
            {
                error +=  count + ". Remove duplicate key: '" + x + "' \n";
                count++;
            });

            var validationResult = new ValidationResult()
            {
                IsFailed = error.Length > 0,
                Error = error
            };
            return validationResult;
        }
    }

    public class ValidationResult
    {
        public bool IsFailed { get; set; }
        public string Error { get; set; }

    }
}
