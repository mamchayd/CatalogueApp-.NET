using System.ComponentModel.DataAnnotations;

namespace CatalogueApp.Models
{

    public class Client{
        [Key]
        public int ClientID{get;set;}
        public string Name{get;set;}
        public string Email{get;set;}
        public string Adress{get;set;}
    }
    
}