namespace LibraryManagementSystem.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime ReturnDate => BorrowDate.AddDays(15);
    }
}