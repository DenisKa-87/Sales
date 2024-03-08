using Api.Entities;

namespace Api.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBookById(int id);
        Task<Book> GetBookByIsbn(long isbn);
        Task<IEnumerable<Book>> GetBooks();
        Task<bool> AddBook(Book book);
        Task<Book> ChangeBookQuantityInReserve(long isbn, string action);
    }
}
