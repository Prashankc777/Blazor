using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employeemanagement.modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Modals;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception)
            {
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, "Error retrieving the data");
            }


        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;


            }
            catch (Exception e)
            {
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, "Error retrieving the data");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee is null) return BadRequest();
                var emailCheck = _employeeRepository.GetEmail(employee.Email);
                if (emailCheck is null)
                {
                    var createdEmployee = await _employeeRepository.AddEmployee(employee);
                    return CreatedAtAction(nameof(GetEmployee),
                        new { id = createdEmployee.EmployeeId }, createdEmployee);
                }
                ModelState.AddModelError("Email", "Employee already in use");
                return BadRequest(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest("Employee Id Mismatch");
                }
                var employeeToUpdate = await _employeeRepository.GetEmployee(id);
                if (employeeToUpdate is null)
                {
                    return NotFound($"Employee with name {employee.FirstName} not found");
                }
                return await _employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error Updating  employee record");
            }
        }







    }
}
