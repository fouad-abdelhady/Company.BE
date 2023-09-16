namespace Company.PL.Hubs
{
    public interface IConnectedUsersService
    {
        void AddUser(int userId, string connectionId);
        void RemoveUser(int userId);
        string? GetConnectionId(int userId);
    }
}
