using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESportskiCentar.Attributes;

namespace ESportskiCentar.Models
{
    public class Termin
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Datum i vrijeme termina su obavezni.")]
        [DisplayName("Datum termina")]
        [SamoBuducnost(ErrorMessage = "Termin se ne može kreirati u prošlosti! Izaberite buduće vrijeme.")]
        public DateTime datum { get; set; }

        [Required(ErrorMessage = "Cijena termina je obavezna.")]
        [Range(1.0, 500.0, ErrorMessage = "Cijena termina mora biti između 1 i 500 KM.")] 
        [DisplayName("Cijena termina")]
        public double cijena { get; set; }

        [Required]
        [DisplayName("Status termina")]
        public bool rezervisan { get; set; } = false; // default je slobodan

        [Required(ErrorMessage = "Izbor terena je obavezan.")]
        [DisplayName("Teren")]
        [ForeignKey("Teren")]
        public int terenID { get; set; }

        public Teren Teren { get; set; }
    }
}