﻿using Microsoft.EntityFrameworkCore;
using Api.Entities;
using Api.Interfaces;
using SQLitePCL;

namespace Api.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddBook(Book book)
        {
            bool result = false;
            var existingBook = await _context.Books.FirstOrDefaultAsync(x => x.Isbn == book.Isbn);
            if(existingBook != null)
            {
                existingBook.Quantity = book.Quantity;
                existingBook.Price = book.Price;
                existingBook.Year = book.Year;
                existingBook.Author = book.Author;
                existingBook.Title = book.Title;
                return true;
            }
            
              return await _context.Books.AddAsync(book) != null;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.ToArrayAsync();
        }

        public async Task<Book> GetBookById(int id) => await _context.Books.FirstOrDefaultAsync(book => book.Id == id);

        public async Task<Book> GetBookByIsbn(long isbn) => await _context.Books.FirstOrDefaultAsync(book => book.Isbn == isbn);

        public async Task<Book> ChangeBookQuantityInReserve(long isbn, string action)
        {
            var book = await GetBookByIsbn(isbn);
            if (book == null || book.Quantity < 1)
                return null;
            switch (action)
            {
                case "increase":
                    book.InReserve = book.InReserve + 1;
                    break;
                case "decrease":
                    book.InReserve = (book.InReserve - 1 < 0) ? 0 : book.InReserve - 1;
                    break;
            };
            _context.Books.Attach(book);
            _context.Entry(book).Property(x => x.InReserve).IsModified = true;
            _context.SaveChanges();
            return book;
        }
    }
}
