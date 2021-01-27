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
        public async Task<ActionResult> add([FromBody] Category category)
        {
            catalogueDbRepository.categories.Add(category);
            catalogueDbRepository.SaveChanges();

            string serializedCategory = JsonConvert.SerializeObject(category);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = "Add category : "+serializedCategory });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }
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
        public async Task<ActionResult> delete(int id)
        {

            Category category=catalogueDbRepository.categories.Find(id);
            catalogueDbRepository.categories.Remove(category);
            catalogueDbRepository.SaveChanges();

            string serializedCategory = JsonConvert.SerializeObject(category);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = "Delete category : " + serializedCategory });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> update(int id, [FromBody] Category category)
        {
            string serializedCategory_before = JsonConvert.SerializeObject(category);
            category.CategoryID = id;
            catalogueDbRepository.categories.Update(category);
            catalogueDbRepository.SaveChanges();

            string serializedCategory = JsonConvert.SerializeObject(category);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { 
                    Value = "Update category from " + serializedCategory_before + " to "+ serializedCategory 
                });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }
        }

    }
}
