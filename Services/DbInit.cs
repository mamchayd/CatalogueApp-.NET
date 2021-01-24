using CatalogueApp.Models;
using System;

namespace CatalogueApp.Services{
    public class DbInit{
        public static void initData(CatalogueDbRepository catalogueDb){

            Console.WriteLine("Data initialisation.....");
            catalogueDb.categories.Add(new Category{Name="Ordinateur"});
            catalogueDb.categories.Add(new Category{Name="Imprimante"});
            catalogueDb.products.Add(new Product{Name="Imprimante",Price=100,CategoryID=2});
            catalogueDb.products.Add(new Product{Name="Mac",Price=200,CategoryID=1});
            catalogueDb.products.Add(new Product{Name="Ord HP",Price=300,CategoryID=1});

            catalogueDb.SaveChanges();

        }
    }
}