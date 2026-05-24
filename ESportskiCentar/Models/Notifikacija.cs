using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Notifikacija
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Status slanja")]
        public bool poslana { get; set; }
        [DisplayName("Predviđeno vrijeme slanja")]
        public DateTime vrijemeSlanja { get; set; }
        [ForeignKey("Rezervacija")]
        [DisplayName("Rezervacija")]
        public int rezervacijaID { get; set; }

        public Rezervacija Rezervacija { get; set; }

    }
}
