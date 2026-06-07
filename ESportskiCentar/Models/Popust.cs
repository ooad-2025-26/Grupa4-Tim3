using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Popust
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Potreban broj rezervacija je obavezan.")]
        [Range(1, int.MaxValue, ErrorMessage = "Potreban broj rezervacija mora biti najmanje 1.")]
        [DisplayName("Potreban broj rezervacija")]
        public int potrebanBrojRezervacija { get; set; }

        [Required(ErrorMessage = "Procenat popusta je obavezan.")]
        [Range(0.00, 100.00, ErrorMessage = "Procenat popusta mora biti između 0 i 100.")]
        [DisplayName("Procenat popusta")]
        public double procenat { get; set; }
    }
}
