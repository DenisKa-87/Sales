using Api.Interfaces;
using System.Collections.Concurrent;

namespace Api.SignalR
{
    public class ConnectedUsers : IConnectedUsers
    {
        public ConcurrentDictionary<string, List<string>> UserConnections { get; }

        public ConnectedUsers()
        {
            UserConnections = new ConcurrentDictionary<string, List<string>>();
        }
    }
}
