using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaulterList.Models
{
    public class ContextDefaulter : DbContext
    {
        public ContextDefaulter() : base("ConStr") { }

        public DbSet<TotalList> TotalLists { get; set; }
        public DbSet<Defaulter> Defaulters { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Result> Results { get; set; }

        public DbSet<Dictionary> Dictionaries { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ContextDefaulter>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
