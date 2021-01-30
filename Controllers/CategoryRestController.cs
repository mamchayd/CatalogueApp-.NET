using CatalogueApp.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CatalogueApp.Controllers
{

    [Route("api/categories")]
    public class CategoryRestController: Controller
    {
        public CatalogueDbRepository catalogueDbRepository { get; set; }
        private string topic = "facturation";
        private ProducerConfig _config;
        public CategoryRestController(ProducerConfig _config, CatalogueDbRepository repository)
        {
            this._config = _config;
            this.catalogueDbRepository = repository;
        }
        [HttpGet]
        public IEnumerable<Category> list()
        {
            return catalogueDbRepository.categories;
        }
        [HttpPost]
        public Category add([FromBody] Category category)
        {
            catalogueDbRepository.categories.Add(category);
            catalogueDbRepository.SaveChanges();

           return category;
        }
        [HttpGet("{id}")]
        public Category find(int id)
        {
            return catalogueDbRepository.categories.FirstOrDefault(s=> s.CategoryID==id);
        }
        [HttpGet("{id}/products")]
        public IEnumerable<Product> findProducts(int id)
        {
            Category category = catalogueDbRepository.categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryID == id);
            return category.Products;
        }
        [HttpDelete("{id}")]
        public void delete(int id)
        {

            Category category=catalogueDbRepository.categories.Find(id);
            catalogueDbRepository.categories.Remove(category);
            catalogueDbRepository.SaveChanges();
        }
        [HttpPut("{id}")]
        public Category update(int id, [FromBody] Category category)
        {
            category.CategoryID = id;
            catalogueDbRepository.categories.Update(category);
            catalogueDbRepository.SaveChanges();

          return category;
        }

    }
}
