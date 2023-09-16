using System.Collections.Concurrent;

namespace Company.PL.Hubs
{
    public class ConnectedUsersService : IConnectedUsersService
    {
        static private ConnectedUsersService _connectedUserService;

        private ConnectedUsersService() { }
        static public  ConnectedUsersService GetInstance() {
            if (_connectedUserService == null) _connectedUserService = new ConnectedUsersService();
            return _connectedUserService;
        }
        private readonly ConcurrentDictionary<int, string> _userConnections = new ConcurrentDictionary<int, string>();
        public void AddUser(int userId, string connectionId)
        {
            _userConnections.AddOrUpdate(userId, connectionId, (key, oldValue) => connectionId);
        }

        public string? GetConnectionId(int userId)
        {
            if(_userConnections.TryGetValue(userId, out var connectionId)) 
                return connectionId;
            return null;
        }

        public void RemoveUser(int userId)
        {
            _userConnections.TryRemove(userId, out _);
        }
    }
}
