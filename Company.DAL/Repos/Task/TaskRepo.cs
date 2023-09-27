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
        public const int RECIEVED = 0;
        public const int SEEN = 1;
        public const int ON_PROGRESS = 2;
        public const int DONE = 3;
        public const int GRADED = 4;
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

        public int CountItems(int userId, string role, string? keyword)
        {
            if (role.ToLower() == "employee") 
                return _companyContext.Tasks.Where((tsk) => tsk.EmployeeId == userId && (
                        EF.Functions.Like(tsk.Title, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(tsk.ArTitle, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(tsk.Description, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(tsk.ArDescription, $"%{keyword ?? ""}%")
                        )).Count();
       
            if (role.ToLower() == "manager") 
                return _companyContext.Tasks.Where ((tsk) => tsk.CreatorId == userId && (
                        EF.Functions.Like(tsk.Title, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(tsk.ArTitle, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(tsk.Description, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(tsk.ArDescription, $"%{keyword ?? ""}%")
                        )).Count();
            
            return 0;
        }

        public List<Data.Models.Task> GetEmployeeTasks(int employeeId, int page, int limit, int caller, string? keyword)
        {
            if (caller == 1) {
                var employeeTasks = _companyContext.Tasks.Where((tsk) => tsk.EmployeeId == employeeId && tsk.Status == 0 ).ToList();
                foreach (var employeeTask in employeeTasks)
                {
                    employeeTask.Status = SEEN;
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
                    .Where(employeeTask => employeeTask.EmployeeId == employeeId && 
                        (
                        EF.Functions.Like(employeeTask.Title, $"%{keyword??""}%") ||
                        EF.Functions.Like(employeeTask.ArTitle, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(employeeTask.Description, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(employeeTask.ArDescription, $"%{keyword ?? ""}%") 
                        )
                      )
                    .OrderBy(tsk => tsk.Status)
                    .Skip(skip)
                    .Take(limit)
                    .ToList();
        }

        public List<Data.Models.Task> GetManagerTasks(int staffMemeberId, int page, int limit, string? keyword)
        {
            int skip = (page - 1) * limit;
            return _companyContext.Tasks.Include(tsk => tsk.Employee)
                    .Where(managerTasks => managerTasks.CreatorId == staffMemeberId &&
                        (
                        EF.Functions.Like(managerTasks.Title, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(managerTasks.ArTitle, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(managerTasks.Description, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(managerTasks.ArDescription, $"%{keyword ?? ""}%") ||
                        EF.Functions.Like(managerTasks.Employee.FullName, $"%{keyword ?? ""}%")
                        ))
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

        public Data.Models.Task? UpdateGrade(int taskId, int state)
        {
            Data.Models.Task? task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId && tsk.Status >= 3);
            if (task == null) return null;
            task.Grade = state;
            task.Status = 4;
            try {
                _companyContext.SaveChanges();
                return task;
            } catch (Exception e) {
                return null;
            }
        }
        // 0 sent, 1 seen, 2 onprogress, 3 done, 4 graded 
        public Data.Models.Task? UpdateStatus(int taskId, int state, int employeeId)
        {
            Data.Models.Task? task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId && tsk.EmployeeId == employeeId);
            if (task == null) return null;
            task.Status = state;
            task.StateChangedAt = DateTime.Now;
            try
            {
                _companyContext.SaveChanges();
                return task;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool SetTasksToSeen(int employeeId) {
            var employeeTasks = _companyContext.Tasks.Where((tsk) => tsk.EmployeeId == employeeId && tsk.Status == RECIEVED).ToList();
            foreach (var employeeTask in employeeTasks) employeeTask.Status = SEEN;
            try
            {
                _companyContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool updateChanges(int taskId, string changes, string arChanges)
        {
            var task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId);
            if (task == null || task.Status < DONE) return false;
            task.Changes = changes;
            task.ArChanges = arChanges;
            try {
                _companyContext.SaveChanges();
                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool SetTaskToSeen(int taskId)
        {
            var task = _companyContext.Tasks.FirstOrDefault(tsk => tsk.Id == taskId);
           
            try {
                if (task.Status >= SEEN) return true;
                task.Status = SEEN;
                _companyContext.SaveChanges();
                return true;
            } catch (Exception e) {
                return false;
            }
        }
    }
}
