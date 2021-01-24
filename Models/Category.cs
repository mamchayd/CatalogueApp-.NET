using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogueApp.Models{
    //Mapi l'entite ajouter annotation table
    [Table("CATEGORIES")]
    public class Category{
        [Key]
        public int CategoryID{get;set;}
        public string Name {get;set;}

    }
}
