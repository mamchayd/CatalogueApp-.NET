using CatalogueApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CatalogueApp.Controllers{

    [Route("/api/products")]
    public class ProductRestController : Controller{
        //unite de persistance
        public CatalogueDbRepository catalogueRepository { get; set; }
        //ingection des dependance par constrecture
        public ProductRestController(CatalogueDbRepository repozitory){
            this.catalogueRepository=repozitory;
        }

        [HttpGet]
        public IEnumerable<Product> findAll(){
            return catalogueRepository.products.Include(p=>p.category);
        }


     [HttpGet("paginate")]
         public IEnumerable<Product> page(int page=0,int size=1){
            int skipValue=(page-1)*size;
            return catalogueRepository
                        .products
                        .Include(p=>p.category)
                        .Skip(skipValue)
                        .Take(size);
        }
        

         [HttpGet("search")]
        public IEnumerable<Product> search(string kw){
            return catalogueRepository
                        .products
                        .Include(p=>p.category)
                        .Where(p=>p.Name.Contains(kw));
        }

        [HttpGet("{Id}")]
        public Product getOne(int Id){
            return catalogueRepository.products.Include(p=>p.category)
            .FirstOrDefault(p=>p.productID==Id);
        }

        [HttpPost]
        public Product save([FromBody]Product product){
            catalogueRepository.products.Add(product);
            catalogueRepository.SaveChanges();
            return product;
        }

        [HttpPut("{Id}")]
        public Product update([FromBody]Product product,int Id){
            product.productID=Id; 
            catalogueRepository.products.Update(product);
            catalogueRepository.SaveChanges();
            return product;
        }

        [HttpDelete("{Id}")]
        public void Delete(int Id){
            Product product=catalogueRepository.products.FirstOrDefault(p=>p.productID==Id);
            catalogueRepository.Remove(product);
            catalogueRepository.SaveChanges();
          
        }
     
        
    }
}