using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.DomainModel
{
    public class Culture
    {
        public int KeyID { get; set; }
        public string RFCTag { get; set; }
        public string Name { get; set; }
    }
}
