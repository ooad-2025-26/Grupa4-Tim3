using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Termin
    {
        [Key]
        public int id { get; set; }

        [DisplayName("Datum termina")]
        public DateTime datum { get; set; }

        [DisplayName("Cijena termina")]
        public double cijena { get; set; }

        [DisplayName("Status termina")]
        public bool rezervisan { get; set; }

        [DisplayName("Teren")]
        [ForeignKey("Teren")]
        public int terenID { get; set; }

        public Teren? Teren { get; set; }
    }
}
