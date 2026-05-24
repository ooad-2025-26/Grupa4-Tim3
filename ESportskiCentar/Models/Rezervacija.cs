using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Rezervacija
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Status")]
        public Status status { get; set; }

        [ForeignKey("Termin")]
        [DisplayName("Termin")]
        public int terminID { get; set; }

        [ForeignKey("Korisnik")]
        [DisplayName("Korisnik")]
        public String korisnikID { get; set; }


        [DisplayName("Datum i vrijeme rezervacije")]
        public DateTime vrijemeRezervacije { get; set; }

        [DisplayName("Popust")]
        public bool primjenjenPopust { get; set; }

        [DisplayName("Ukupna cijena")]
        public double konacnaCijena { get; set; }

        public Termin Termin { get; set; }
        public Korisnik Korisnik { get; set; }

    }
}
