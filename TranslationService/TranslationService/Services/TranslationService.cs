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
                       }).OrderBy(x => x.Key).ToListAsync();

            return list;
        }

        public async Task SaveTranslationForCulture(int cultureId, List<TranslationVO> translations)
        {
            var translationKeys = translations.Select(x => x.Key);

            //fetch persisted transltion keys
            var existingTranslationKeys = await _db.TranslationKeys.Where(x => translationKeys.Contains(x.Key)).ToListAsync();

            var existingTranlationKeyIds = existingTranslationKeys.Select(x => x.KeyID);

            //fetch persisted translation values
            var existingTranslationValues = await _db.TranslationValues.Where(x => existingTranlationKeyIds.Contains(x.TranslationKeyID) && x.CultureID == cultureId).ToListAsync();

            var existingTranslationPairs = (from key in existingTranslationKeys
                                           join value in translations
                                           on key.Key equals value.Key
                                           select new
                                           {
                                               key.KeyID,
                                               value.Value
                                           }).ToDictionary(x => x.KeyID, y => y.Value);


            //update existing translation value
            foreach (var translation in existingTranslationValues)
            {
                translation.Value = existingTranslationPairs[translation.TranslationKeyID];
            }

            //add new translation keys
            var newKeys = translationKeys.Where(x => !existingTranslationKeys.Select(y => y.Key).Contains(x)).ToList();
            var newTranslationKeys = new List<TranslationKey>();
            var newTranslationPairs = translations.Where(x => newKeys.Contains(x.Key));

            foreach (var translation in newTranslationPairs)
            {
                newTranslationKeys.Add(new TranslationKey()
                {
                    Key = translation.Key
                });
            }

            _db.TranslationKeys.AddRange(newTranslationKeys);
            await _db.SaveChangesAsync();

            //add new translation values
            var translationKeyDict = newTranslationKeys.ToDictionary(x => x.Key, y => y.KeyID);
            var newTranslationValues = new List<TranslationValue>();

            foreach (var translation in newTranslationPairs)
            {
               newTranslationValues.Add(new TranslationValue()
               {
                    TranslationKeyID = translationKeyDict[translation.Key],
                    CultureID = cultureId,
                    Value = translation.Value == null ? string.Empty : translation.Value
               });
            }

            _db.TranslationValues.AddRange(newTranslationValues);
            await _db.SaveChangesAsync();

            return;
        }
    }
}
