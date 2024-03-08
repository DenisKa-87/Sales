using System.Runtime.CompilerServices;

namespace Api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public  AppUser User { get; set; }
        public List<Book> Books { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime PlacedAt { get; set; } = DateTime.Now;

        // shows if user has finished books selection.
        public bool Placed { get; set; }
        // shows if this order data has been sent to the main site;
        public bool Processed { get; set; }


        public static Order Create(AppUser appUser)
        {
            Order order = new Order
            {
                User = appUser,
                Books = new List<Book>()
            };
            return order;
        }
        public static Order Create(AppUser user, List<Book> books)
        {
            var order = new Order
            {
                User = user,
                Books = books
            };
            return order;
        }

        public  bool AddBook(Book book)
        {
            if (Books == null)
            {
                Books = new List<Book>{book};
                return true;
            }
            if(Books.Contains(book))
            {
                return false;
            }
            Books.Add(book);
            return true;

        }

        public bool RemoveBook(Book book)
        {
            if (Books == null)
            {
                return false;
            }
            if (Books.Contains(book))
            {
                Books.Remove(book);
                return true;
            }
            return false;
        }


    }
}
