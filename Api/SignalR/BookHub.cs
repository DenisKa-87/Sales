using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Api.Data;
using Api.DTO;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.SignalR
{

    public class BookHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookHub(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
        }

   
        
       
        public async void NotifyBookQuantityChange(long isbn)
        {
            var book = await _unitOfWork.BookRepository.GetBookByIsbn(isbn);
            await Clients.All.SendAsync("updateBook", BookDto.Create(book));
        }
    }
}
