using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.TaskDtos;
using Company.BL.Managers.NotificationManagers;
using Company.BL.Managers.TaskManagers;
using Company.PL.Filter;
using Company.PL.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Company.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskManager _taskManager;
        private IHubContext<NewTaskAssignmentHub> _hubContext;
        private INotificationManager _notificationManager;
        public TaskController(ITaskManager taskManager, IHubContext<NewTaskAssignmentHub> hubContext, INotificationManager notificationManager) {
            _taskManager = taskManager;
            _hubContext = hubContext;
            _notificationManager = notificationManager;
        }
        [HttpPost]
        [Authorize]
        [ManagerAuth]
        //we have what we need
        public async Task<ActionResult<TaskRes>> AddTask(NewTaskReq newTask) {

            int.TryParse(User.FindFirst("UserId").Value, out int managerId);
            TaskResContainer newTasks = _taskManager.AddTask(newTask, managerId);
            int unseenNotificationsCount = _notificationManager.AddNotification(managerId, newTask.EmployeeId, newTasks.taskRes.Id, (int)INotificationManager.NotificationTypes.TASK_INSERTION);
            await NotifyUsersAsync(newTask.EmployeeId, unseenNotificationsCount);
            return Ok(newTasks.taskRes);
        }

        [HttpPut]
        [Route("State")]
        [Authorize]
        [EmployeeAuth]
        [StatusValidation]
        public async Task<ActionResult<ResultDto>> UpdateTaskStatus([FromBody] TaskStateUpdate taskStatusUpdate)
        {
            int.TryParse(User.FindFirst("UserId").Value, out int employeeId);
            ResultDto result = _taskManager.UpdateStatus(taskStatusUpdate, employeeId);
            if (!result.State) return Ok(result);
            int unseenNotificationsCount = _notificationManager.AddNotification(employeeId, result.OptionalNum??-1, taskStatusUpdate.TaskId, (int)INotificationManager.NotificationTypes.TASK_UPDATE);
            await NotifyUsersAsync(result.OptionalNum??-1, unseenNotificationsCount);
            return Ok(result);
        }

        [HttpPut]
        [Route("Grade")]
        [Authorize]
        [ManagerAuth]
        [GradeValidation]
        //the employee Id in the optional num in the result object
        public async Task<ActionResult<ResultDto>> UpdateTaskGrade(TaskStateUpdate taskGradeUpdate) {
            int.TryParse(User.FindFirst("UserId").Value, out int managerId);
            ResultDto result = _taskManager.UpdateGrade(taskGradeUpdate);
            if (!result.State) return Ok(result);
            int unseenNotificationsCount = _notificationManager.AddNotification(managerId, result.OptionalNum ?? -1, taskGradeUpdate.TaskId, (int)INotificationManager.NotificationTypes.TASK_UPDATE);
            await NotifyUsersAsync(result.OptionalNum ?? -1, unseenNotificationsCount);
            return Ok(result);
        }

        [HttpGet]
        [Route("{employeeId}")]
        [Authorize]
        [ManagerAuth]
        [PaginationValidator]
        public ActionResult<TasksRes> GetTasksByEmployeeId(int employeeId, [FromQuery] int page = 1, [FromQuery] int limit = 12, [FromQuery] string? keywords = "") {
            TasksRes result = _taskManager.GetEmployeeTasks(employeeId, page, limit, 2,keywords??"");
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [ManagerEmployeeAuth]
        [PaginationValidator]
        public ActionResult<TasksRes> GetCallerTasks([FromQuery] int page = 1, [FromQuery] int limit = 12, [FromQuery] string? keyword="") {
            int.TryParse(User.FindFirst("UserId").Value, out int userId);
            string role = User.FindFirst("Role").Value;
            TasksRes result = _taskManager.GetCallerTasks(userId, role, page, limit, keyword);
            return Ok(result);
        }

        [HttpGet]
        [Route("State")]
        [Authorize]
        [ManagerAuth]
        public ActionResult<List<TaskRes>> GetTasksByState([FromQuery]int employeeId,[FromQuery]int state) {
            List<TaskRes> tasksList = _taskManager.GetTaskByState(employeeId, state);
            return Ok(tasksList);
        }

        [HttpGet]
        [Route("UnseenCount")]
        [Authorize]
        [EmployeeAuth]
        public ActionResult<int> GetUnseenTasksCount() {
            int.TryParse(User.FindFirst("UserId").Value, out int userId);
            int? unseenTasksCount = _taskManager.GetUnseenTasksCount(userId);
            return Ok(unseenTasksCount??0);
        }

        [HttpPut]
        [Route("RequestChanges")]
        [Authorize]
        [ManagerAuth]
        public async Task<ActionResult<ResultDto>> UpdateRequestChangesAsync(UpdateChanges changes) {
            bool result = _taskManager.updateChanges(changes.taskId, changes.changes, changes.arChanges);
            if (!result) return Ok(new ResultDto(State: false, Message: "Make sure the task is done."));
            int.TryParse(User.FindFirst("UserId").Value, out int managerId);
            int unseenNotificationsCount = _notificationManager.AddNotification(managerId, changes.employeeId, changes.taskId, (int)INotificationManager.NotificationTypes.TASK_CHANGE_REQUEST);
            await NotifyUsersAsync(changes.employeeId, unseenNotificationsCount);
            return Ok(new ResultDto(State: true, Message: "Changes Requested."));
        }

        private async Task NotifyUsersAsync(int staffMemberId, int notificationsCount)
        {
            string? connectionId = ConnectedUsersService.GetInstance().GetConnectionId(staffMemberId);
            if (connectionId != null)
                await _hubContext.Clients.Client(connectionId).SendAsync("NotifyEmployee", notificationsCount);
        }

    }
}
