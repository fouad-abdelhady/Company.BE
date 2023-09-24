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
            var task = new Task() 
            { 
                Title = newTask.Title, 
                ArTitle = newTask.ArTitle, 
                Description = newTask.Description,
                ArDescription = newTask.ArDescription,
                EmployeeId = newTask.EmployeeId, 
                CreatorId = managerId
            };
            TaskResContainer1? taskInfo = _taskRepo.AddTask(task);
            return new TaskResContainer(tasksCount: taskInfo.tasksCount, taskRes: new TaskRes(
                Id: taskInfo.taskRes.Id, 
                Title: taskInfo.taskRes.Title,
                ArTitle: taskInfo.taskRes.ArTitle,
                Description: taskInfo.taskRes.Description,
                ArDescription: taskInfo.taskRes.ArDescription,
                Status: taskInfo.taskRes.Status,
                StaffMember:null,
                Grade:taskInfo.taskRes.Grade,
                Changes: taskInfo.taskRes.Changes,
                CreatedAt: DateTime.Now,
                LastStateChange: DateTime.Now,
                ArChanges: taskInfo.taskRes.ArChanges
                )); ;
        }

        public TasksRes GetEmployeeTasks(int employeeId, int page, int limit, int caller, string? keyword)
        {
            List<Task> tasksList = _taskRepo.GetEmployeeTasks(employeeId, page, limit, caller, keyword);
            return new TasksRes(
                CallerId: employeeId,
                pageInfo: _GetPageInfo(page, employeeId, limit, "employee", keyword),
                tasksList: _TransformTasksList(tasksList, "employee")
                );
        }

        public TasksRes GetCallerTasks(int staffMemeberId, string role, int page, int limit, string?keyword)
        {
            if (role.ToLower() == "employee")
                return GetEmployeeTasks(staffMemeberId, page, limit, 1, keyword);
            List<Task> StaffMemeberTask = _taskRepo.GetManagerTasks(staffMemeberId, page, limit, keyword);
            return new TasksRes(
                CallerId: staffMemeberId,
                pageInfo: _GetPageInfo(page, staffMemeberId, limit, "manager", keyword),
                tasksList: _TransformTasksList(StaffMemeberTask, "manager")
            );
        }

        public ResultDto UpdateGrade(TaskStateUpdate taskGradeUpdate)
        {
            Task result = _taskRepo.UpdateGrade(taskGradeUpdate.TaskId, taskGradeUpdate.State);
            return  new ResultDto(
                State: result != null, 
                Message: result != null ? "Updated Successfully" : "Error Occured", 
                OptionalNum: result != null ? result.EmployeeId : null); ;
        }

        public ResultDto UpdateStatus(TaskStateUpdate taskStatusUpdate, int employeeId)
        {
            Task result = _taskRepo.UpdateStatus(taskStatusUpdate.TaskId, taskStatusUpdate.State, employeeId);
            return new ResultDto(
                State: result != null, 
                Message: result != null? "Updated Successfully" : "Error Occured", 
                OptionalNum: result !=null? result.CreatorId: null) ;
        }

        private PageInfo _GetPageInfo(int page, int userId, int limit, string? role, string?keyword) {
            int itemsCount = _taskRepo.CountItems(userId, role??"employee", keyword);
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
                ArTitle : tsk.ArTitle,
                Description : tsk.Description,
                ArDescription : tsk.ArDescription,
                Status : tsk.Status,
                StaffMember : _MapUser(tsk.Creator?? tsk.Employee),
                Grade : tsk.Grade,
                Changes: tsk.Changes,
                CreatedAt : tsk.CreatedAt,
                LastStateChange : tsk.StateChangedAt,
                ArChanges: tsk.ArChanges
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
                    ArTitle: tsk.ArTitle,
                    Description: tsk.Description,
                    ArDescription: tsk.ArDescription,
                    Status: tsk.Status,
                    StaffMember: _MapUser(tsk.Employee),
                    Grade: tsk.Grade,
                    Changes: tsk.Changes,
                    CreatedAt: tsk.CreatedAt,
                    LastStateChange: tsk.StateChangedAt,
                    ArChanges: tsk.ArChanges
                )).ToList();
        }

        public bool updateChanges(int taskId, string changes, string arChanges)
        {
            bool result = _taskRepo.updateChanges(taskId, changes, arChanges);
            return result;
        }
    }
}
