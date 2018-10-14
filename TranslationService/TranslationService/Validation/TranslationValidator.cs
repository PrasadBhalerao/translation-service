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
        public ValidationResult Validate(TranslationDTO translations)
        {
            var error = "";
            var count = 1;
            
            //no rows
            if (translations.Translations.Count == 0)
            {
                return new ValidationResult()
                {
                    IsFailed = true,
                    Error = "Add translations"
                };
            }

            //duplicate keys
            var duplicateItems = translations.Translations.GroupBy(x => x.Key).Where(y => y.Count() > 1).SelectMany(z => z);

            var duplicateKeys = duplicateItems.Select(x => x.Key).Distinct().ToList();

            
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
}
