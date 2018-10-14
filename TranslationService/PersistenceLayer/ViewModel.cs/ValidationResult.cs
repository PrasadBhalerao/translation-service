using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.ViewModel.cs
{
    public class ValidationResult
    {
        public bool IsFailed { get; set; }
        public string Error { get; set; }
    }
}
