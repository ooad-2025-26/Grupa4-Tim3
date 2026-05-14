using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Korisnik:IdentityUser
    {
        public string ime { get; set; }
        public string prezime { get; set; }
      
    }
}
