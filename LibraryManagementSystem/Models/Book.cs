namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public bool IsEnable { get; set; } = true;
        public ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}