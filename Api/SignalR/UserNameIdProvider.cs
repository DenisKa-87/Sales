using Microsoft.AspNetCore.SignalR;

namespace Api.SignalR
{
    public class UserNameIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var user = connection.User;
            return user?.FindFirst("Name")?.Value;
        }
    }
}
