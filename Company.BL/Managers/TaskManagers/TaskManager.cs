using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.CommonDtos;
using Company.BL.Dtos.StaffDtos;
using Company.BL.Dtos.TaskDtos;
using Company.DAL.Data.Dto;
using Company.DAL.Repos.Task;
using Task = Company.DAL.Data.Models.Task;

namespace Company.BL.Managers.TaskManagers
{
    public class TaskManager : ITaskManager
    {
        private ITaskRepo _taskRepo;
        public TaskManager(ITaskRepo taskRepo) { 
            _taskRepo = taskRepo;
        }
        public TaskResContainer AddTask(NewTaskReq newTask, int managerId)
        {
            var task = new Task() { Title = newTask.Title, Description = newTask.Description, EmployeeId = newTask.EmployeeId, CreatorId = managerId};
            TaskResContainer1? taskInfo = _taskRepo.AddTask(task);
            return new TaskResContainer(tasksCount: taskInfo.tasksCount, taskRes: new TaskRes(
                Id: taskInfo.taskRes.Id, 
                Title: taskInfo.taskRes.Title,
                Description: taskInfo.taskRes.Description,
                Status: taskInfo.taskRes.Status,
                StaffMember:null,
                Grade:taskInfo.taskRes.Grade,
                CreatedAt: DateTime.Now,
                LastStateChange: DateTime.Now
                )); ;
        }

        public TasksRes GetEmployeeTasks(int employeeId, int page, int limit, int caller)
        {
            List<Task> tasksList = _taskRepo.GetEmployeeTasks(employeeId, page, limit, caller);
            return new TasksRes(
                CallerId: employeeId,
                pageInfo: _GetPageInfo(page, employeeId, limit, "employee"),
                tasksList: _TransformTasksList(tasksList, "employee")
                );
        }

        public TasksRes GetCallerTasks(int staffMemeberId, string role, int page, int limit)
        {
            if (role.ToLower() == "employee")
                return GetEmployeeTasks(staffMemeberId, page, limit, 1);
            List<Task> StaffMemeberTask = _taskRepo.GetManagerTasks(staffMemeberId, page, limit);
            return new TasksRes(
                CallerId: staffMemeberId,
                pageInfo: _GetPageInfo(page, staffMemeberId, limit, "manager"),
                tasksList: _TransformTasksList(StaffMemeberTask, "manager")
            );
        }

        public ResultDto UpdateGrade(TaskStateUpdate taskGradeUpdate)
        {
            bool result = _taskRepo.UpdateGrade(taskGradeUpdate.TaskId, taskGradeUpdate.State);
            return new ResultDto(State: result, Message: result ? "Updated Successfully" : "The Task May not completed yet.");
        }

        public ResultDto UpdateStatus(TaskStateUpdate taskStatusUpdate, int employeeId)
        {
            bool result = _taskRepo.UpdateStatus(taskStatusUpdate.TaskId, taskStatusUpdate.State, employeeId);
            return new ResultDto(State: result, Message: result ? "Updated Successfully" : "Error Occured");
        }

        private PageInfo _GetPageInfo(int page, int userId, int limit, string? role) {
            int itemsCount = _taskRepo.CountItems(userId, role??"employee");
            int pagesCount = (int)Math.Ceiling((double)itemsCount / limit);
            int? nextPage = page >= pagesCount ? null : page + 1;
            int? previous = page <= 1?  null : page - 1;
            int current = page;
            return new PageInfo(Next: nextPage, Previous: previous, Current: current, PagesCount: pagesCount);
        }
   
        private List<TaskRes> _TransformTasksList(List<Task> tasksList, string role)
        {
            return tasksList.Select(tsk => new TaskRes(
                Id : tsk.Id,
                Title : tsk.Title,
                Description : tsk.Description,
                Status : tsk.Status,
                StaffMember : _MapUser(tsk.Creator?? tsk.Employee),
                Grade : tsk.Grade,
                CreatedAt : tsk.CreatedAt,
                LastStateChange : tsk.StateChangedAt
                )).ToList();
        }

        private StaffRead _MapUser(DAL.Data.Models.Staff staffMember)
        {
            return new StaffRead(Id: staffMember.Id,
                Name: staffMember.FullName,
                Email: staffMember.EmailAddress,
                Role: staffMember.Role,
                Image: staffMember.Image);
        }

        public int? GetUnseenTasksCount(int userId)
        {
            return _taskRepo.GetUnssenTasksCount(userId);
        }

        public List<TaskRes> GetTaskByState(int employeeId, int state)
        {
            List<Task> dbTasksList = _taskRepo.GetTasksByState(employeeId, state);
            return dbTasksList.Select(tsk=> new TaskRes(
                    Id: tsk.Id,
                    Title: tsk.Title,
                    Description: tsk.Description,
                    Status: tsk.Status,
                    StaffMember: _MapUser(tsk.Employee),
                    Grade: tsk.Grade,
                    CreatedAt: tsk.CreatedAt,
                    LastStateChange: tsk.StateChangedAt
                )).ToList();
        }
    }
}
