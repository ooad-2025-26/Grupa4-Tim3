using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Notifikacija
    {
        [Key]
        public int id { get; set; }
        public bool poslana { get; set; }
        public DateTime vrijemeSlanja { get; set; }
        [ForeignKey("Rezervacija")]
        public int rezervacijaID { get; set; }

        public Rezervacija Rezervacija { get; set; }

    }
}
