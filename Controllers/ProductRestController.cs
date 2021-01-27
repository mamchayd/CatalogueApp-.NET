using CatalogueApp.Models;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogueApp.Controllers
{   [Route("/api/products")]
    public class ProductRestController : Controller
    {
        public CatalogueDbRepository catalogueDbRepository { get; set; }
        private string topic = "facture";
        private ProducerConfig _config;

        public ProductRestController(ProducerConfig _config, CatalogueDbRepository repository)
        {
            this._config = _config;
            this.catalogueDbRepository = repository;
        }
        [HttpGet]
        public IEnumerable<Product> list()
        {
            catalogueDbRepository.products.Include(p=>p.category);
            return catalogueDbRepository.products;
        }
        [HttpPost]
        public async Task<ActionResult> add([FromBody] Product product)
        {
            catalogueDbRepository.products.Add(product);
            catalogueDbRepository.SaveChanges();

            string serializedProduct = JsonConvert.SerializeObject(product);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = "Add product : " + serializedProduct });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }
        }
        [HttpGet("search")]
        public IEnumerable<Product> search(string kw)
        {
            return catalogueDbRepository
                .products
                .Include(p => p.category)
                .Where(p=>p.Name.Contains(kw));
        }
        [HttpGet("paginate")]
        public IEnumerable<Product> page(int page, int size)
        {
            int skip = (page - 1) * size;
            return catalogueDbRepository.products.Include(p=>p.category).Skip(skip).Take(size);
        }
        [HttpGet("{id}")]
        public Product find(int id)
        {
            return catalogueDbRepository.products.Include(p=>p.category).FirstOrDefault(c=> c.productID==id);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> delete(int id)
        {
            Product product = catalogueDbRepository.products.Find(id);
            catalogueDbRepository.products.Remove(product);
            catalogueDbRepository.SaveChanges();

            string serializedProduct = JsonConvert.SerializeObject(product);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = "supp product : " + serializedProduct });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> update(int id, [FromBody] Product product)
        {
            string serializedProduct_before = JsonConvert.SerializeObject(product);
            product.productID = id;
            catalogueDbRepository.products.Update(product);
            catalogueDbRepository.SaveChanges();

            string serializedProduct = JsonConvert.SerializeObject(product);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { 
                    Value = "Update product from " + serializedProduct_before + " to " + serializedProduct 
                });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }
        }

    }
}
