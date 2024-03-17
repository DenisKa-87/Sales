using System.Collections.Concurrent;

namespace Api.Interfaces
{
    public interface IConnectedUsers
    {
        public ConcurrentDictionary<string, List<string>> UserConnections { get; }
    }
}
