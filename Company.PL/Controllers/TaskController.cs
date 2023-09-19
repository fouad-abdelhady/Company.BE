using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.TaskDtos;
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
        public TaskController(ITaskManager taskManager, IHubContext<NewTaskAssignmentHub> hubContext) {
            _taskManager = taskManager;
            _hubContext = hubContext;
        }
        [HttpPost]
        [Authorize]
        [ManagerAuth]
        public async Task<ActionResult<TaskRes>> AddTask(NewTaskReq newTask) {

            int.TryParse(User.FindFirst("UserId").Value, out int managerId);
            TaskResContainer newTasks = _taskManager.AddTask(newTask, managerId);
            string? connectionId = ConnectedUsersService.GetInstance().GetConnectionId(newTask.EmployeeId);
            if (connectionId !=null)
                await _hubContext.Clients.Client(connectionId).SendAsync("NotifyEmployee", newTasks.tasksCount);
            return Ok(newTasks.taskRes);
        }

        [HttpPut]
        [Route("State")]
        [Authorize]
        [EmployeeAuth]
        [StatusValidation]
        public ActionResult<ResultDto> UpdateTaskStatus([FromBody]TaskStateUpdate taskStatusUpdate) {
            int.TryParse(User.FindFirst("UserId").Value, out int employeeId);
            ResultDto result = _taskManager.UpdateStatus(taskStatusUpdate, employeeId);
            return Ok(result);
        }

        [HttpPut]
        [Route("Grade")]
        [Authorize]
        [ManagerAuth]
        [GradeValidation]
        public ActionResult<ResultDto> UpdateTaskGrade(TaskStateUpdate taskGradeUpdate) {
            ResultDto result = _taskManager.UpdateGrade(taskGradeUpdate);
            return Ok(result);
        }

        [HttpGet]
        [Route("{employeeId}")]
        [Authorize]
        [ManagerAuth]
        [PaginationValidator]
        public ActionResult<TasksRes> GetTasksByEmployeeId(int employeeId, [FromQuery] int page = 1, [FromQuery] int limit = 12) {
            TasksRes result = _taskManager.GetEmployeeTasks(employeeId, page, limit);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [ManagerEmployeeAuth]
        [PaginationValidator]
        public ActionResult<TasksRes> GetCallerTasks([FromQuery] int page = 1, [FromQuery] int limit = 12) {
            int.TryParse(User.FindFirst("UserId").Value, out int userId);
            string role = User.FindFirst("Role").Value;
            TasksRes result = _taskManager.GetCallerTasks(userId, role, page, limit);
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

    }
}
