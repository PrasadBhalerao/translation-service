using PersistenceLayer.ViewModel.cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TranslationService.Validation
{
    public class ReportValidator
    {
        public ValidationResult Validate(HttpPostedFile file)
        {
            var error = "";
            var count = 1;

            if (file == null || file.ContentLength == 0)
            {
                error += count + ". Please select a file \n";
                count++;
            }

            if (file != null && file.FileName.Split('.').Last() != "xlsx")
            {
                error += count + ". Only excel files with '.xlsx' extensions can be added! \n";
                count++;
            }

            var validationResult = new ValidationResult()
            {
                IsFailed = error.Length > 0,
                Error = error
            };
            return validationResult;
        }
    }
}
