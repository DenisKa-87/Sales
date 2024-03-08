using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Api.Data;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Api.DTO;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Api.Controllers
{
    public class BooksController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        //returns all available books from repository
        public async Task<ActionResult<List<BookDto>>> GetItems()
        {
            var items = await _unitOfWork.BookRepository.GetBooks();
            var booksDtos = new List<BookDto>();
            foreach (var book in items)
            {
                var bookAvailable = book.Quantity - book.InReserve;
                if (bookAvailable == 0)
                    continue;
                booksDtos.Add(BookDto.Create(book));
            }
            return Ok(booksDtos);
        }


        //[HttpPost("add")]
        //[Authorize(Policy = "RequiresAdminRole")]
        //public async Task<ActionResult<IEnumerable<Book>>> AddBooks(IEnumerable<BookDto> bookDtos)
        //{
        //    var addedBooks = new List<Book>();
        //    foreach (var bookDto in bookDtos)
        //    {
        //        var book = new Book();
        //        try
        //        {
        //             book = Book.Create(bookDto);
        //        }
        //        catch (Exception ex)
        //        {
                    
        //            continue;
        //            //throw new InvalidDataException("Something is wrong with provided data");
        //        }
        //        var result = await _unitOfWork.BookRepository.AddBook(book);
        //        if(result)
        //            addedBooks.Add(book);
        //    }
        //    if (await _unitOfWork.Complete())
        //        return Ok(addedBooks);
        //    return BadRequest();
        //}
    }
}


