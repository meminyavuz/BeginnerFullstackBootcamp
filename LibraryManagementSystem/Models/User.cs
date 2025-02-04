namespace LibraryManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}