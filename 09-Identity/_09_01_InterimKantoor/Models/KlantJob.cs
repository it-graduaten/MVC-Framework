using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _09_01_InterimKantoor.Models
{
    public class KlantJob
    {
        [Key]
        public int Id { get; set; }
        public string KlantId { get; set; } = default!;
        public int JobId { get; set; }

        public Klant Klant { get; set; } = new Klant()!;

        public Job Job { get; set; } = new Job()!;



    }
}
