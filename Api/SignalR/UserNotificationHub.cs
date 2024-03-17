using Api.Data;
using Api.DTO;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Api.SignalR
{
    [Authorize]
    public class UserNotificationHub : Hub  
    {
        private readonly IConnectedUsers _connectedUsers;
        private readonly UserManager<AppUser> _userManager;

        public UserNotificationHub( IConnectedUsers connectedUsers, UserManager<AppUser> userManager)
        {
            _connectedUsers = connectedUsers;
            _userManager = userManager;
        }


        public async override Task OnConnectedAsync()
        {

            var name = Context.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == name);

            if (_connectedUsers.UserConnections.ContainsKey(name))
            {
                _connectedUsers.UserConnections[name].Add(connectionId);
            }
            else
            {
                _connectedUsers.UserConnections[name] = new List<string>() { connectionId };
            }
            // return base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = Context.ConnectionId;
            var name = Context.User.Identity.Name;
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == Context.User.Identity.Name.ToUpper());

            if (name == null)
            {
                return;
                //return base.OnConnectedAsync();
            }
            if (_connectedUsers.UserConnections.ContainsKey(name))
            {
                _connectedUsers.UserConnections[name].RemoveAll(x => x == connectionId);
            }
            // return base.OnConnectedAsync();
        }

        public async void UpdateOrder()
        {
            var name = Context.User.Identity.Name;
            var connections = _connectedUsers.UserConnections[name];
            if (connections == null)
                return;

            foreach(var connection in connections) 
            { 
               await Clients.Client(connection).SendAsync("updateOrder");
            }
            
        }
    }
}
