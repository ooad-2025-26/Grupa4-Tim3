using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Teren
    {
        [Key]
        public int id { get; set; }
        public string naziv { get; set; }
        public Sport sport { get; set; }
    }
}
