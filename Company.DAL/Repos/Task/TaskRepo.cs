using Company.DAL.Data.Context;
using Company.DAL.Data.Dto;
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
        public TaskResContainer1? AddTask(Data.Models.Task task)
        {
            _companyContext.Tasks.Add(task);
            try {
                _companyContext.SaveChanges();
            } catch(Exception e) {
                Console.WriteLine(e);
            }
            int unseenTasksCount = _companyContext.Tasks.Where(tsk => tsk.Status == 0 && tsk.EmployeeId == task.EmployeeId).Count();
            TaskResContainer1 taskResContainer = new TaskResContainer1(tasksCount: unseenTasksCount, taskRes: task);
            return taskResContainer;
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

        public List<Data.Models.Task> GetTasksByState(int employeeId, int state)
        {
            return _companyContext.Tasks.Include(tsk => tsk.Employee).Where(task => task.EmployeeId == employeeId && task.Status == state).ToList();
        }

        public int? GetUnssenTasksCount(int userId)
        {
            return _companyContext.Tasks.Where(tsk => tsk.Status == 0 && tsk.EmployeeId == userId).Count();
        }

        public bool UpdateGrade(int taskId, int state)
        {
            Data.Models.Task? task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId && tsk.Status == 3);
            if (task == null) return false;
            task.Grade = state;
            task.Status = 4;
            try {
                _companyContext.SaveChanges();
                return true;
            } catch (Exception e) {
                return false;
            }
        }
        // 0 sent, 1 seen, 2 onprogress, 3 done, 4 graded 
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
