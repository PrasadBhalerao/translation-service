using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.DomainModel
{
    public class TranslationKey
    {
        [Key]
        public int KeyID { get; set; }
        public string Key { get; set; }
    }
}
