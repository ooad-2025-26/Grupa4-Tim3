using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Korisnik:IdentityUser
    {
        [DisplayName("Ime")]
        public string ime { get; set; }


        [DisplayName("Prezime")]
        [RegularExpression(@"^[A-Za-zČčĆćŽžŠšĐđ\s-]{2,30}$")]
        public string prezime { get; set; }
      
    }
}
