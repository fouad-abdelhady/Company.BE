using Company.BL.Dtos.AuthDtos;
using Company.BL.Dtos.TaskDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BL.Managers.TaskManagers
{
    public interface ITaskManager
    {
        TaskResContainer AddTask(NewTaskReq newTask, int managerId);
        ResultDto UpdateStatus(TaskStateUpdate taskStatusUpdate, int employeeId);
        ResultDto UpdateGrade(TaskStateUpdate taskGradeUpdate);
        /**
         * the caller used to whether include the manager  data or employee data in the response
         * 
         * **/
        TasksRes GetEmployeeTasks(int employeeId, int page, int limit, int caller = 2, string keyword = "");
        TasksRes GetCallerTasks(int employeeId, string role, int page, int limit, string? keyword);
        int? GetUnseenTasksCount(int userId);
        List<TaskRes> GetTaskByState(int employeeId, int state);
        bool updateChanges(int taskId, string changes, string arChanges);
    }
}
