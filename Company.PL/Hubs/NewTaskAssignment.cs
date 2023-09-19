using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Company.PL.Hubs
{
   // [Authorize]
    public class NewTaskAssignmentHub : Hub
    {
        private ConnectedUsersService _connectedUsersService;
        public NewTaskAssignmentHub() { 
            _connectedUsersService = ConnectedUsersService.GetInstance();
        }
        public override async Task OnConnectedAsync()
        {

          /*  int? userId = _extractUserId();
            if (userId != null) _connectedUsersService.AddUser(userId??-1, Context.ConnectionId);
            await Clients.All.SendAsync("User Joined", $"Employee Id = {userId}");
            await base.OnConnectedAsync();*/
        }
        [HubMethodName("NotifyEmployee")]
        public async Task NotifyEmployee(int employeeId)
        {
            // int? userId = _extractUserId();
            Console.WriteLine("Employee Registered" + employeeId);
            if (employeeId != null) _connectedUsersService.AddUser(employeeId, Context.ConnectionId);
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
