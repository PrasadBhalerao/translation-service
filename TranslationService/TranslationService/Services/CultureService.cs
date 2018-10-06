using PersistenceLayer;
using PersistenceLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationService.Services
{
    public class CultureService
    {
        private DataContext _db;
        public CultureService()
        {
            _db = new DataContext();
        }

        public async Task<List<Culture>> GetCultures(string searchString)
        {
            var list = await _db.Cultures.ToListAsync();
            return list;
        }
    }
}
