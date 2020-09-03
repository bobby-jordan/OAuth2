using OAuth2Example.DAL.Models;
using System.Data.Entity;

namespace OAuth2Example.DAL
{
    internal class OAuth2ExampleDbContext : DbContext
    {
        public OAuth2ExampleDbContext() : base("OAuth2ExampleDb")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.UniqueIndexFor<User>(u => u.Email);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuthToken> Tokens { get; set; }
    }
}
