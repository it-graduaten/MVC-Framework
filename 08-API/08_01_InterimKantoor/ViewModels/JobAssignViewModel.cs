using _08_01_InterimKantoor.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace _08_01_InterimKantoor.ViewModels
{
    public class JobAssignViewModel
    {
        public string KlantId { get; set; } = default!;
        public int JobId { get; set; } 
        public List<SelectListItem> Klanten { get; set; } = new List<SelectListItem>();
        public string JobOmschrijving { get; set; } = default!;
    }
}
