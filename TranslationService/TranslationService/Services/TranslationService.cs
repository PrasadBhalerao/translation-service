using PersistenceLayer;
using PersistenceLayer.DomainModel;
using PersistenceLayer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
            var list = await (from translationKey in _db.TranslationKeys
                              join translationValue in _db.TranslationValues.Where(x => x.CultureID == cultureId)
                              on translationKey.KeyID equals translationValue.TranslationKeyID into translatedValues
                              from translatedValue in translatedValues.DefaultIfEmpty()
                       select new TranslationVO
                       {
                           Key = translationKey.Key,
                           Value = translatedValue.Value,
                           TranslationKeyId = translationKey.KeyID,
                           TranslationValueId = translatedValue.KeyID
                       }).OrderBy(x => x.Key).ToListAsync();

            return list;
        }

        public async Task SaveTranslationForCulture(int cultureId, List<TranslationVO> translations)
        {
            var persistedTranslations = await GetTranslationsForCulture(cultureId);

            var persistedKeys = persistedTranslations.Select(x => x.Key);
            var newKeys = translations.Where(x => !persistedKeys.Contains(x.Key)).Select(x => x.Key).ToList();
            var newTranslationKeys = new List<TranslationKey>();
            //add new keys
            foreach (var key in newKeys)
            {
                newTranslationKeys.Add(new TranslationKey()
                {
                    Key = key
                });
            }
            _db.TranslationKeys.AddRange(newTranslationKeys);
            await _db.SaveChangesAsync();

            var newTranslationValues = new List<TranslationValue>();
            //add new values
            foreach (var key in newTranslationKeys)
            {
                newTranslationValues.Add(new TranslationValue()
                {
                    TranslationKeyID = key.KeyID,
                    CultureID = cultureId,
                    Value = translations.Single(x => x.Key == key.Key).Value
                });
            }
            _db.TranslationValues.AddRange(newTranslationValues);

            //add existing key translation values
            var existingKeyTranslations = persistedTranslations.Where(x => !newKeys.Contains(x.Key) && x.TranslationValueId == null);
            foreach (var translationValue in existingKeyTranslations)
            {
                var value = translations.Single(x => x.Key == translationValue.Key).Value;
                var translationValueObj = new TranslationValue()
                {
                    CultureID = cultureId,
                    KeyID = 0,
                    TranslationKeyID = translationValue.TranslationKeyId ?? 0,
                    Value = value == null ? string.Empty : value
                };
                _db.TranslationValues.Add(translationValueObj);
            };
            await _db.SaveChangesAsync();

            //update existing translation values
            foreach (var translationValue in persistedTranslations.Where(x => x.TranslationValueId != null))
            {
                var value = translations.SingleOrDefault(x => x.Key == translationValue.Key)?.Value;
                var translationValueObj = new TranslationValue()
                {                    
                    CultureID = cultureId,
                    KeyID = translationValue.TranslationValueId ?? 0,
                    TranslationKeyID = translationValue.TranslationKeyId ?? 0,
                    Value = value == null  ? string.Empty : value
                };
                _db.Entry(translationValueObj).State = EntityState.Modified;
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            }
            return;
        }
    }
}
