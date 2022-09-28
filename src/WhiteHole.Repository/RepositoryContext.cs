using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteHole.Repository.Models;

namespace WhiteHole.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<WhiteHoleObject> WhiteHoleObjects { get; set; }
        public DbSet<WhiteHoleObjectKV> WhiteHoleObjectKVs { get; set; }
        public DbSet<WhiteHoleObjectRelation> WhiteHoleObjectRelations { get; set; }
    }
}
