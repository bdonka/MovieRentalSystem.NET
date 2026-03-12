using MovieRentalSystem.NET.WebApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieRentalSystem.NET.WebApi.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } = null!; // now i know, this is navigation
        [Required]
        public int MoviePhysicalCopyId { get; set; }
        public MoviePhysicalCopy MoviePhysicalCopy { get; set; } = null!; // this is navigation too
        public DateTime RentalDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7); // the 7 days is an example too because we haven't established it
        public DateTime? ReturnDate { get; set; }
        [Range(0, 1000)] // the upper limit is just an example because we haven't established it
        public decimal TotalPrice { get; set; }
    }
}
