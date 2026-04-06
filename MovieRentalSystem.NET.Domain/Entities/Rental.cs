using MovieRentalSystem.NET.Domain.Enums;

namespace MovieRentalSystem.NET.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int MoviePhysicalCopyId { get; set; }
        public MoviePhysicalCopy MoviePhysicalCopy { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? RentalStartDate { get; set; } 
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public RentalStatus Status { get; set; } = RentalStatus.Preparing;
    }
}
