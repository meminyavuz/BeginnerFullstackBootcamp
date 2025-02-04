namespace LibraryManagementSystem.Models
{
    public class BorrowViewModel
    {
        public int UserId { get; set; }
        public List<Book> AvailableBooks { get; set; }
        public List<Borrow> BorrowedBooks { get; set; }  
    }

}