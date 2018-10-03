using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.ValueObjects
{
    public class TranslationVO
    {
        public int KeyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int ValueKeyId { get; set; }
    }
}
