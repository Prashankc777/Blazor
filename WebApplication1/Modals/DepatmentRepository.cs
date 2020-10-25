using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employeemanagement.modals;

namespace WebApplication1.Modals
{
    public interface IDepatmentRepository
    {
        IEnumerable<Deparment> GetDepartments();
        Deparment GetDepartment(int departmentId);
    }

    public class DepartmentRepository : IDepatmentRepository

    {
        private readonly AppDbcontext appDbContext;

        public DepartmentRepository(AppDbcontext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Deparment GetDepartment(int departmentId)
        {
            return appDbContext.Deparments
                .FirstOrDefault(d => d.DepartmentId == departmentId);
        }

        public IEnumerable<Deparment> GetDepartments()
        {
            return appDbContext.Deparments;
        }
    }
}
