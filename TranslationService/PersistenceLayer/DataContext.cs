using PersistenceLayer.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace PersistenceLayer
{
    public class DataContext : DbContext
    {
        public DataContext(): base("DatabaseConnString")
        {
        }

        public DbSet<Culture> Cultures { get; set; }
        public DbSet<TranslationKey> TranslationKeys { get; set; }
        public DbSet<TranslationValue> TranslationValues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
