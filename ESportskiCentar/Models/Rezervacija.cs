using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Rezervacija
    {
        [Key]
        public int id { get; set; }
        public Status status { get; set; }
        [ForeignKey("Termin")]
        public int terminID { get; set; }
        [ForeignKey("Korisnik")]
        public int korisnikID { get; set; }
        public DateTime vrijemeRezervacije { get; set; }
        public bool primjenjenPopust { get; set; }
        public double konacnaCijena { get; set; }

        public Termin Termin { get; set; }
        public Korisnik Korisnik { get; set; }

    }
}
