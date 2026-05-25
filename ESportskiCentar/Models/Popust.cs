using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Popust
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Potreban broj rezervacija")]
        public int potrebanBrojRezervacija { get; set; }
        [DisplayName("Procenat popusta")]
        public double procenat { get; set; } = 0;
    }
}
