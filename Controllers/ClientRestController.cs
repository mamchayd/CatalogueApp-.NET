using System.Collections.Generic;
using System.Linq;
using CatalogueApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogueApp.Controllers{
    
        [Route("/api/clients")]

    public class ClientRestController : Controller{

        CatalogueDbRepository catalogueRepository{get;set;}

        public ClientRestController(CatalogueDbRepository repozitory){
                this.catalogueRepository=repozitory;
        }


        [HttpGet]
        public IEnumerable<Client> findAllClient(){
            return catalogueRepository.clients;
        }
        [HttpGet("{Id}")]
        public Client getOneClient(int Id){
            return catalogueRepository.clients.FirstOrDefault(c=>c.ClientID==Id);
        }
       

        [HttpPost]
        public Client save([FromBody]Client client){
            catalogueRepository.clients.Add(client);
            catalogueRepository.SaveChanges();
            return client;
        }

        [HttpPut("{Id}")]
        public Client update([FromBody]Client client,int Id){
            client.ClientID=Id; 
            catalogueRepository.clients.Update(client);
            catalogueRepository.SaveChanges();
            return client;
        }

        [HttpDelete("{Id}")]
        public void Delete(int Id){
            Client client=catalogueRepository.clients.FirstOrDefault(c=>c.ClientID==Id);
            catalogueRepository.Remove(client);
            catalogueRepository.SaveChanges();
          
        }
     
        
    }
}