using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Teren
    {
        [Key]
        public int id { get; set; }

        [DisplayName("Naziv terena")]
        public string naziv { get; set; }

        [DisplayName("Sport")]
        public Sport sport { get; set; }
    }
}
