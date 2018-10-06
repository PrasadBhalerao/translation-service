using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.DomainModel
{
    public class Culture
    {
        [Key]
        public int KeyID { get; set; }
        public string CultureName { get; set; }
        public string CultureCode { get; set; }
        public string DisplayName { get; set; }
    }
}
