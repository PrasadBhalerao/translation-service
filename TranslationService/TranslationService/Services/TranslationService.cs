using PersistenceLayer;
using PersistenceLayer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationService.Services
{
    public class TranslationService
    {
        private DataContext _db;
        public TranslationService()
        {
            _db = new DataContext();
        }


        public async Task<List<TranslationVO>> GetTranslationsForCulture()
        {
            var list = await(from translationKey in _db.TranslationKeys
                       join translationValue in _db.TranslationValues
                       on translationKey.KeyID equals translationValue.TranslationKeyID
                       join culture in _db.Cultures
                       on translationValue.CultureID equals culture.KeyID
                       select new TranslationVO
                       {
                           Key = translationKey.Key,
                           Value = translationValue.Value,
                           KeyId = translationKey.KeyID,
                           ValueKeyId = translationValue.KeyID
                       }).ToListAsync();

            return list;
        }

        public async Task<bool> SaveTranslationForCulture(int cultureId, List<TranslationVO> translations)
        {
            var list = await (from translationKey in _db.TranslationKeys
                              join translationValue in _db.TranslationValues
                              on translationKey.KeyID equals translationValue.TranslationKeyID
                              join culture in _db.Cultures
                              on translationValue.CultureID equals culture.KeyID
                              select new TranslationVO
                              {
                                  Key = translationKey.Key,
                                  Value = translationValue.Value,
                                  KeyId = translationKey.KeyID,
                                  ValueKeyId = translationValue.KeyID
                              }).ToListAsync();
            return false;
        }

    }
}
