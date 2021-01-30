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
{   [Route("/api/clients")]
    public class ClientsRestRepository: Controller
    {
        public CatalogueDbRepository catalogueDbRepository { get; set; }
        private string topic = "facturation";
        private ProducerConfig _config;

        public ClientsRestRepository(ProducerConfig _config, CatalogueDbRepository repository)
        {
            this._config = _config;
            this.catalogueDbRepository = repository;
        }
        [HttpGet]
        public IEnumerable<Client> list()
        {
            return catalogueDbRepository.clients;
        }
        [HttpPost]
        public Client add([FromBody] Client client)
        {
            catalogueDbRepository.clients.Add(client);
            catalogueDbRepository.SaveChanges();

          return client;
        }
       /* public async Task<ActionResult> add([FromBody] Client client)
        {
            catalogueDbRepository.clients.Add(client);
            catalogueDbRepository.SaveChanges();

            string serializedClient = JsonConvert.SerializeObject(client);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = "ajouter client : " + serializedClient });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }
        }*/
        [HttpGet("{id}")]
        public Client find(int id)
        {
            return catalogueDbRepository.clients.FirstOrDefault(c=> c.ClientID==id);
        }
        [HttpDelete("{id}")]
         public void delete(int id)
        {
            Client client=catalogueDbRepository.clients.Find(id);
            catalogueDbRepository.clients.Remove(client);
            catalogueDbRepository.SaveChanges();
       }
       /* public async Task<ActionResult> delete(int id)
        {
            Client client=catalogueDbRepository.clients.Find(id);
            catalogueDbRepository.clients.Remove(client);
            catalogueDbRepository.SaveChanges();

            string serializedClient = JsonConvert.SerializeObject(client);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = "Suppression de client : " + serializedClient });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }

        }*/
        [HttpPut("{id}")]
         public Client update(int id, [FromBody] Client client)
        {
            client.ClientID = id;
            catalogueDbRepository.clients.Update(client);
            catalogueDbRepository.SaveChanges();
            return client;
        }
        /*
        public async Task<ActionResult> update(int id, [FromBody] Client client)
        {
            string serializedClient_before = JsonConvert.SerializeObject(client);
            client.ClientID = id;
            catalogueDbRepository.clients.Update(client);
            catalogueDbRepository.SaveChanges();

            string serializedClient = JsonConvert.SerializeObject(client);
            using (var producer = new ProducerBuilder<Null, string>(_config).Build())
            {
                await producer.ProduceAsync(topic, new Message<Null, string> { 
                    Value = "Update Client from " + serializedClient_before + " to " + serializedClient 
                });
                producer.Flush(TimeSpan.FromSeconds(10));
                return Ok(true);
            }
        }*/

    }
}
