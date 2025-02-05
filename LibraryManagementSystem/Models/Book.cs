namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int NumOfPage { get; set; }
        public string? Genre { get; set; }
        public int PublishYear { get; set; }
        public bool IsEnable { get; set; } = true;
        public ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}