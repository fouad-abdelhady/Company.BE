using Company.DAL.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = Company.DAL.Data.Models.Task;
namespace Company.DAL.Repos.Task
{
    public class TaskRepo : ITaskRepo
    {
        private readonly CompanyContext _companyContext;
        public TaskRepo(CompanyContext companyContext) { 
            _companyContext = companyContext;
        }
        public int? AddTask(Data.Models.Task task)
        {
            _companyContext.Tasks.Add(task);
            _companyContext.SaveChanges();
            int unseenTasksCount = _companyContext.Tasks.Where(tsk => tsk.Status == 0).Count();
            return unseenTasksCount;
        }

        public int CountItems(int userId, string role)
        {
            if (role.ToLower() == "employee") 
                return _companyContext.Tasks.Where((tsk) => tsk.EmployeeId == userId).Count();
       
            if (role.ToLower() == "manager") 
                return _companyContext.Tasks.Where ((tsk) => tsk.CreatorId == userId).Count();
            
            return 0;
        }

        public List<Data.Models.Task> GetEmployeeTasks(int employeeId, int page, int limit, int caller)
        {
            if (caller == 1) {
                var employeeTasks = _companyContext.Tasks.Where((tsk) => tsk.EmployeeId == employeeId && tsk.Status == 0).ToList();
                foreach (var employeeTask in employeeTasks)
                {
                    employeeTask.Status = 1;
                }
                try
                {
                    _companyContext.SaveChanges();
                }
                catch (Exception ex)
                {
                }
            }
            int skip = (page - 1) * limit;
            return _companyContext.Tasks.Include(tsk => caller == 1?  tsk.Creator : tsk.Employee)
                    .Where(employeeTask => employeeTask.EmployeeId == employeeId)
                    .OrderBy(tsk => tsk.Status)
                    .Skip(skip)
                    .Take(limit)
                    .ToList();
        }

        public List<Data.Models.Task> GetManagerTasks(int staffMemeberId, int page, int limit)
        {
            int skip = (page - 1) * limit;
            return _companyContext.Tasks.Include(tsk => tsk.Employee)
                    .Where(managerTasks => managerTasks.CreatorId == staffMemeberId)
                    .OrderBy(tsk => tsk.Status)
                    .Skip(skip)
                    .Take(limit)
                    .ToList();
        }

        public bool UpdateGrade(int taskId, int state)
        {
            Data.Models.Task? task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId && tsk.Status == 5);
            if (task == null) return false;
            task.Grade = state;
            try {
                _companyContext.SaveChanges();
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public bool UpdateStatus(int taskId, int state, int employeeId)
        {
            Data.Models.Task? task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId && tsk.EmployeeId == employeeId);
            if (task == null) return false;
            task.Status = state;
            task.StateChangedAt = DateTime.Now;
            try
            {
                _companyContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
