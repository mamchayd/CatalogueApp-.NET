using CatalogueApp.Models;
using System;

namespace CatalogueApp.Services{
    public class DbInit{
        public static void initData(CatalogueDbRepository catalogueDb){

            Console.WriteLine("Data initialisation.....");
            catalogueDb.categories.Add(new Category{Name="Ordinateur"});
            catalogueDb.categories.Add(new Category{Name="Imprimante"});
            catalogueDb.SaveChanges();

        }
    }
}