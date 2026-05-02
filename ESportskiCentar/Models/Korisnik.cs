using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Korisnik
    {
        [Key]
        public int id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string lozinka { get; set; }
        public string korisnickoIme { get; set; }
        public string email { get; set; }
    }
}
