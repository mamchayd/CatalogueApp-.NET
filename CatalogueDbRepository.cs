using CatalogueApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogueApp
{
    public class CatalogueDbRepository : DbContext
    {
        public DbSet<Category> categories{get;set;}
        public DbSet<Product> products{get;set;}
        public CatalogueDbRepository(DbContextOptions options):base (options){

        }
    }
}