using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Catering.Models.ViewModels
{
    public class CatererListItemViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public int MinGuests { get; set; }    
    }

    public class CatererSearchViewModel
    {
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Number of Guests")]
        public int? NumGuests { get; set; }

        public List<CatererListItemViewModel> Results { get; set; }
            = new List<CatererListItemViewModel>();
    }
}
