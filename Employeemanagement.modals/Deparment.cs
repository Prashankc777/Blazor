using System;
using System.ComponentModel.DataAnnotations;

namespace Employeemanagement.modals
{
    public class Deparment
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
