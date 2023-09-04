using InterimKantoor.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InterimKantoor.ViewModels
{
    public class JobEditViewModel
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
        public List<KlantJobListViewModel> KlantJobs { get; set; } = new List<KlantJobListViewModel>();

        public List<SelectListItem> Klanten { get; set; } = new List<SelectListItem>();
        
    }

}
