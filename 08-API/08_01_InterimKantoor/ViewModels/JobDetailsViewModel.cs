using _08_01_InterimKantoor.Models;
using System.ComponentModel.DataAnnotations;

namespace _08_01_InterimKantoor.ViewModels
{
    public class JobDetailsViewModel
    {
        public int Id { get; set; }
        public string Omschrijving { get; set; } = default!;
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public string Locatie { get; set; } = default!;
        public bool IsWerkschoenen { get; set; }
        public bool IsBadge { get; set; }
        public bool IsKleding { get; set; }
        public int AantalPlaatsen { get; set; }
        public int VrijePlaatsen { get; set; }
    }
}
