namespace Catering.Models
{
    public class Favorite
    {
        public int FavoriteId { get; set; }
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public string CatererId { get; set; }
        public ApplicationUser Caterer { get; set; }

        public DateTime AddedOn { get; set; }
    }
}