using InterimKantoor.Models;

namespace InterimKantoor.ViewModels
{
    public class JobDeleteViewModel
    {
        public int JobId { get; set; }
        public string Omschrijving { get; set; } = default!;
    }
}
