using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Termin
    {
        [Key]
        public int id { get; set; }
        public DateTime datum { get; set; }
        public double cijena { get; set; }
        public bool rezervisan { get; set; }
        [ForeignKey("Teren")]
        public int terenID { get; set; }

        public Teren Teren { get; set; }
    }
}
