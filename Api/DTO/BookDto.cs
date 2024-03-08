using salesAPI.Entities;

namespace Api.DTO
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public long Isbn { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Available { get; set; }

        public static BookDto Create(Book book)
        {
            return new BookDto
            {
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Isbn = book.Isbn,
                Image = book.Image,
                Available = book.Quantity - book.InReserve,
                Price = book.Price,
            };
        }
    }
}
