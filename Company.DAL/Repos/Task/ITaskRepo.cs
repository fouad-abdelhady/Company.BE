using Company.DAL.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Company.DAL.Repos.Task
{
    public interface ITaskRepo
    {
        TaskResContainer1? AddTask(Data.Models.Task task);
        int CountItems(int userId, string role, string? keyWord);
        List<Data.Models.Task> GetEmployeeTasks(int employeeId, int page, int limit, int caller, string? keyword);
        List<Data.Models.Task> GetManagerTasks(int staffMemeberId, int page, int limit, string? keyword);
        Data.Models.Task? UpdateGrade(int taskId, int state);
        Data.Models.Task? UpdateStatus(int taskId, int state, int employeeId);
        int? GetUnssenTasksCount(int userId);
        List<Data.Models.Task> GetTasksByState(int employeeId, int state);
        public bool SetTasksToSeen(int employeeId);
        public bool SetTaskToSeen(int taskId);
        bool updateChanges(int taskId, string changes, string arChanges);
    }
}
