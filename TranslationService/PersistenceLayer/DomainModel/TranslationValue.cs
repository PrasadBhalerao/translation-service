using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.DomainModel
{
    public class TranslationValue
    {
        public int KeyID { get; set; }
        public int CultureID { get; set; }
        public int TranslationKeyID { get; set; }
        public string Value { get; set; }

    }
}
