using salesAPI.DTO;

namespace salesAPI.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public long Isbn { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int InReserve { get; set; }
        public List<Order> Orders { get; set; } 
        

        
        public static Book Create(string title, string author, int year, long isbn, string image, double price, int quantity)
        {
            var book = new Book();
            if (title == null || image == null || author == null)
                return null;

            book.Title = title;
            book.Author = author;
            book.Year = year;
            book.Isbn = isbn;
            book.Image = image;
            book.Price = price;
            book.Quantity = quantity;
            book.Orders = new List<Order>();

            return book;
        }
    }
}
