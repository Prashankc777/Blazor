using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employeemanagement.modals;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Modals
{
    public interface IEmployeeRepository
    {

        Task<IEnumerable<Employee>> Search(string name, Gender? gender);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployee(int employeeId);
        Task<Employee> GetEmail(string email);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<Employee> DeleteEmployee(int employeeId);
    }



    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbcontext appDbContext;

        public EmployeeRepository(AppDbcontext appDbcontext)
        {
            appDbContext = appDbcontext;
        }

        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> query = appDbContext.Employees;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }

            if (gender != null)
            {
                query = query.Where(e => e.Gender == gender);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await appDbContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await appDbContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<Employee> GetEmail(string email)
        {
            return await appDbContext.Employees.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var result = await appDbContext.Employees.AddAsync(employee);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = await appDbContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            if (result == null) return null;
            result.FirstName = employee.FirstName;
            result.LastName = employee.LastName;
            result.Email = employee.Email;
            result.DateOfBrith = employee.DateOfBrith;
            result.Gender = employee.Gender;
            result.DepartmentId = employee.DepartmentId;
            result.PhotoPath = employee.PhotoPath;

            await appDbContext.SaveChangesAsync();

            return result;

        }

        public async Task<Employee> DeleteEmployee(int employeeId)
        {
            var result = await appDbContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (result == null) return null;
            appDbContext.Employees.Remove(result);
            await appDbContext.SaveChangesAsync();
            return result;

        }
    }


}

