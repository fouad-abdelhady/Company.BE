using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Company.PL.Hubs
{
    [Authorize]
    public class NewTaskAssignmentHub : Hub
    {
        private ConnectedUsersService _connectedUsersService;
        public NewTaskAssignmentHub() { 
            _connectedUsersService = ConnectedUsersService.GetInstance();
        }
        public override async Task OnConnectedAsync()
        {

            int? userId = _extractUserId();
            if (userId != null) _connectedUsersService.AddUser(userId??-1, Context.ConnectionId);
            await Clients.All.SendAsync("User Joined", $"Employee Id = {userId}");
            await base.OnConnectedAsync();
        }
        [HubMethodName("NotifyEmployee")]
        public async Task NotifyEmployee(int employeeId)
        {
            string? connectionId = _connectedUsersService.GetConnectionId(employeeId);
            if (connectionId == null) return;
            await Clients.Client(connectionId).SendAsync("RefreshTasks", "NewTaskAdded");
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int? userId = _extractUserId();
            if (userId != null) _connectedUsersService.RemoveUser(userId ?? -1);
            await base.OnDisconnectedAsync(exception);
        }

        private int? _extractUserId() {
            var userIdStr = Context.User == null || Context.User.FindFirst("UserId") == null ? null : Context.User.FindFirst("UserId").Value;
            if (int.TryParse(userIdStr, out int userId))
                return userId;
            return null;
        }
    }
}
