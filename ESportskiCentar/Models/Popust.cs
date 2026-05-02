using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ESportskiCentar.Models
{
    public class Popust
    {
        [Key]
        public int id { get; set; }
        public int potrebanBrojRezervacija { get; set; }
        public double procenat { get; set; }
    }
}
