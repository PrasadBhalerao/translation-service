using PersistenceLayer;
using PersistenceLayer.DomainModel;
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


        public async Task<List<TranslationVO>> GetTranslationsForCulture(int cultureId)
        {
            var list = await(from translationKey in _db.TranslationKeys
                       join translationValue in _db.TranslationValues.Where(x => x.CultureID == cultureId)
                       on translationKey.KeyID equals translationValue.TranslationKeyID
                       select new TranslationVO
                       {
                           Key = translationKey.Key,
                           Value = translationValue.Value,
                           TranslationKeyId = translationKey.KeyID,
                           TranslationValueId = translationValue.KeyID
                       }).ToListAsync();

            return list;
        }

        public async void SaveTranslationForCulture(int cultureId, List<TranslationVO> translations)
        {
            var newTranslationKeys = new List<TranslationKey>();
            var newTranslationValues = new List<TranslationValue>();

            foreach (var translation in translations)
            {
                if (translation.TranslationKeyId == null)
                {
                    newTranslationKeys.Add(new TranslationKey()
                    {
                        Key = translation.Key
                    });
                }
            }

            _db.TranslationKeys.AddRange(newTranslationKeys);
            await _db.SaveChangesAsync();

            var translationKeyDict = newTranslationKeys.ToDictionary(x => x.Key, y => y.KeyID);

            foreach (var translation in translations)
            {
                if (translation.TranslationValueId == null)
                {
                    newTranslationValues.Add(new TranslationValue()
                    {
                        TranslationKeyID = translationKeyDict[translation.Key],
                        CultureID = cultureId,
                        Value = translation.Value == null ? string.Empty : translation.Value
                    });
                }
            }
            _db.TranslationValues.AddRange(newTranslationValues);
            await _db.SaveChangesAsync();
        }

    }
}
