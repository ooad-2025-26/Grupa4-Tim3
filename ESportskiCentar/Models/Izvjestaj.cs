using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Izvjestaj
    {
        [Key]
        public int id { get; set; }
        public DateTime datumGenerisanja { get; set; }
        [ForeignKey("Korisnik")]
        public int korisnikID { get; set; }
        public DateTime datumOd { get; set; }
        public DateTime datumDo { get; set; }

        public Korisnik Korisnik { get; set; }
    }
}
