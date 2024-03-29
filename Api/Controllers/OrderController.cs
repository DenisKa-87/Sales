﻿using Api.DTO;
using Api.Entities;
using Api.Interfaces;
using Api.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using SQLitePCL;
using System.Security.Policy;

namespace Api.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<BookHub> _bookhubContext;
        private readonly IHubContext<UserNotificationHub> _userNotificationHub;
        private readonly IConnectedUsers _connectedUsers;

        public OrderController(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IHubContext<BookHub> bookhubContext,
        IHubContext<UserNotificationHub> userNotificationHub, IConnectedUsers connectedUsers
            )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _bookhubContext = bookhubContext;
            _userNotificationHub = userNotificationHub;
            _connectedUsers = connectedUsers;
        }

        
        [HttpGet("current")]
        [Authorize]
        //returns current order, if user is logged in
        public async Task<ActionResult<OrderDto>> GetCurrentOrder()
        {
            var user = await GetCurrentUser();
            if (user == null)
            {
                return BadRequest(new { message = "Could not find this user." });
            }
            var order = await _unitOfWork.OrderRepository.GetCurrentOrder(user);

            return Ok(OrderDto.Create(order));
        }

        [Authorize]
        [HttpPost("addBook/{isbn}")]
        //adds book to user's order
        public async Task<ActionResult<OrderDto>> AddBookToOrder(long isbn)
        {
            var book = await _unitOfWork.BookRepository.GetBookByIsbn(isbn);
            if (book == null)
                return BadRequest("No such book");
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("No such user");
            var order = await _unitOfWork.OrderRepository.GetCurrentOrder(user);
            if (order == null)
                return BadRequest("No such order");

            if (book.Quantity - book.InReserve <= 0)
                return BadRequest(new { message = "The book is not available" });
            if (!order.AddBook(book))
            {
                return BadRequest("The book is already added");
            }
            book = await _unitOfWork.BookRepository.ChangeBookQuantityInReserve(book.Isbn, "increase");
            _unitOfWork.OrderRepository.UpdateOrder(order);
            if (await _unitOfWork.Complete())
            {
                return Ok(OrderDto.Create(order));
            }

            return BadRequest();
        }

        [Authorize]
        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteBookFromOrder(long isbn)
        {
            var book = await _unitOfWork.BookRepository.GetBookByIsbn(isbn);
            if (book == null)
                return BadRequest("No such book");
            var user = await GetCurrentUser();
            if (user == null)
                return BadRequest("No such user");
            var order = await _unitOfWork.OrderRepository.GetCurrentOrder(user);
            if (order == null)
                return BadRequest("No such order");
            if (!order.RemoveBook(book))
                return BadRequest("Could not remove book");
            book = await _unitOfWork.BookRepository.ChangeBookQuantityInReserve(book.Isbn, "decrease");
            _unitOfWork.OrderRepository.UpdateOrder(order);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("/getorders/{quantity}")]
        public async Task<ActionResult<IEnumerable<OrdersResponseDto>>> GetOrders(int quantity)
        {
            if (quantity <= 0)
                return BadRequest();
            var orders = await _unitOfWork.OrderRepository.GetActiveOrders(quantity);
            if (orders == null)
                return Ok(null);
            var result = new List<OrdersResponseDto>();
            foreach (var order in orders)
            {
                var dto = OrdersResponseDto.Create(order);
                result.Add(dto);
            }
            return Ok(result);
            
        }

        [Authorize]
        [HttpPost("placeorder")]
        public async Task<IActionResult> PlaceCurrentOrder()
        {
            var user = await GetCurrentUser();
            if(await _unitOfWork.OrderRepository.PlaceCurrentOrder(user))
            {
                if (await _unitOfWork.Complete())
                {
                    return Ok();
                }
            }
            return BadRequest("Failed to place current order.");

        }


        [HttpPost("sendurl")]
        public async Task<IActionResult> SendUrlToUser([FromQuery] string username, [FromQuery] string url)
        {
            var user = await GetUserByName(username);
            var order = user.Order;
            if (user == null | order.Processed | !order.Placed)
                return BadRequest("There is no such user or user has not placed the order");
            order.Processed = true;
            order.OrderUrl = url;
            _unitOfWork.OrderRepository.UpdateOrder(order);
            if (!await _unitOfWork.Complete())
            {
                return BadRequest("Unable to update the order");
            }
            var connectionList = GetUsersConnections(user.NormalizedUserName);
            
            if (connectionList.Count != 0)
            {
                foreach (var connection in connectionList)
                {
                    await _userNotificationHub.Clients.Client(connection).SendAsync("updateOrderUrl", url);
                }
            }
            return Ok();
        }

        [HttpPost("cancelorder")]
        public async Task<IActionResult> CancelOrder(string username)
        {
            var user = await GetUserByName(username);
            if(user == null)
                return BadRequest("There is no such user");
            var order = user.Order;
            if (order == null || !order.Placed || order.Processed)
                return BadRequest("Sorry, there is no such user.");
            bool success = false;
            foreach (var book in order.Books)
            {
                success = await _unitOfWork.BookRepository.ChangeBookQuantityInReserve(book.Isbn, "decrease") != null;
            }
            if (success)
            {
                _unitOfWork.OrderRepository.ClearOrderFromBooks(order);
                if(await _unitOfWork.Complete())
                {
                    await SendNotification(user.UserName, "updateOrder");
                    await SendNotification(user.UserName, "updateOrderUrl");
                }
            }
            return Ok();

        }

        [HttpDelete ("deleteorder")]
        public async Task<IActionResult> DeleteCompletedOrder(string username)
        {
            var user = await GetUserByName(username);
            if (user == null)
                return BadRequest("There is no such user");
            var order = user.Order;
            if (order == null || !order.Processed || !order.Placed) 
                return BadRequest("Sorry, there is no such user.");

            _unitOfWork.OrderRepository.DeleteOrder(order);
            if (!await _unitOfWork.Complete())
                return BadRequest("Unable to delete the order.");

            await SendNotification(user.UserName, "updateOrder");
            return Ok();
        }

        private async Task SendNotification(string username, string notification)
        {
            var connections = GetUsersConnections(username.ToUpper());
            if (connections.Count != 0)
            {
                foreach (var connection in connections)
                {
                    await _userNotificationHub.Clients.Client(connection).SendAsync(notification);
                }
            }
        }
        private async Task<AppUser> GetUserByName(string username)
        {
            var query = _userManager.Users.Include(x => x.Order).ThenInclude(x => x.Books); ;

            return await query.FirstOrDefaultAsync(x => x.NormalizedUserName == username.ToUpper());
        }

        private List<string> GetUsersConnections(string username)
        {
            var connections = new List<string>();
            if (_connectedUsers.UserConnections.ContainsKey(username))
            {
                connections = _connectedUsers.UserConnections[username];
            }
            return connections;

        } 

        private async Task<AppUser> GetCurrentUser()
        {
            var username = User.Identity.Name;
            if (username == null)
                return null;
            var users = _userManager.Users.Include(x => x.Order);
            return await users.FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}
