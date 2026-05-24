using ESportskiCentar.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESportskiCentar.Models
{
    public class Izvjestaj
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Datum generisanja")]
        public DateTime datumGenerisanja { get; set; }
        [DisplayName("Korisnik")]

        [ForeignKey("Korisnik")]
        public String korisnikID { get; set; }
        
        [DisplayName("Od datuma")]
        [ValidacijaRasponaDvaDatuma]
        public DateTime datumOd { get; set; }
        [DisplayName("Do datuma")]

        public DateTime datumDo { get; set; }

        public Korisnik Korisnik { get; set; }
    }
}
