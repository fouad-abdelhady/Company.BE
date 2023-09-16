using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Repos.Task
{
    public interface ITaskRepo
    {
        int? AddTask(Data.Models.Task task);
        int CountItems(int userId, string role);
        List<Data.Models.Task> GetEmployeeTasks(int employeeId, int page, int limit, int caller);
        List<Data.Models.Task> GetManagerTasks(int staffMemeberId, int page, int limit);
        bool UpdateGrade(int taskId, int state);
        bool UpdateStatus(int taskId, int state, int employeeId);
    }
}
