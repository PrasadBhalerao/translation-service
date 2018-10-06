using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.ValueObjects
{
    public class TranslationVO
    {        
        public string Key { get; set; }
        public string Value { get; set; }
        public int? TranslationKeyId { get; set; }
        public int? TranslationValueId { get; set; }
    }
}
