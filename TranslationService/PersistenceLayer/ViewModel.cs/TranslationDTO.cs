using PersistenceLayer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.ViewModel.cs
{
    public class TranslationDTO
    {
        public int CultureId { get; set; }
        public List<TranslationVO> Translations { get; set; }
    }
}
