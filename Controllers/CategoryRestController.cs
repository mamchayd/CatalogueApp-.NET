using CatalogueApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CatalogueApp.Controllers{

    [Route("/api/categories")]
    public class CategoryRestController : Controller{

        //unite de persistance
        public CatalogueDbRepository catalogueRepository { get; set; }
        //ingection des dependance par constrecture
        public CategoryRestController(CatalogueDbRepository repozitory){
            this.catalogueRepository=repozitory;
        }

        [HttpGet]
        public IEnumerable<Category> listCats(){
            return catalogueRepository.categories;
        }

        [HttpGet("{Id}")]
        public Category getcat(int Id){
            return catalogueRepository.categories.FirstOrDefault(c=>c.CategoryID==Id);
        }

        [HttpPost]
        public Category save([FromBody]Category category){
            catalogueRepository.categories.Add(category);
            catalogueRepository.SaveChanges();
            return category;
        }

        [HttpPut("{Id}")]
        public Category update([FromBody]Category category,int Id){
            category.CategoryID=Id; 
            catalogueRepository.categories.Update(category);
            catalogueRepository.SaveChanges();
            return category;
        }

        [HttpDelete("{Id}")]
        public void Delete(int Id){
            Category category=catalogueRepository.categories.FirstOrDefault(c=>c.CategoryID==Id);
            catalogueRepository.Remove(category);
            catalogueRepository.SaveChanges();
          
        }
     
        
    }
}