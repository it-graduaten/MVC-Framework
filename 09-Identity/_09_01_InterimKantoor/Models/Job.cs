using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _09_01_InterimKantoor.Models
{
    public class Job
    {

        public int Id { get; set; }
        [Required]
        public string Omschrijving { get; set; } = default!;
        [Required]
        public DateTime StartDatum { get; set; }
        [Required]
        public DateTime EindDatum { get; set; }
        [Required]
        public string Locatie { get; set; } = default!;
        [Required]
        public bool IsWerkschoenen { get; set; }
        [Required]
        public bool IsBadge { get; set; }
        [Required]
        public bool IsKleding { get; set; }
        [Required]
        public int AantalPlaatsen { get; set; }

        public IList<KlantJob> KlantJobs { get; set; } = new List<KlantJob>()!;

    }
}
