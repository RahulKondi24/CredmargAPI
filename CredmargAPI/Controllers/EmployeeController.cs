using CredmargAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CredmargAPI.Repository;
using Microsoft.AspNetCore.Authorization;

namespace CredmargAPI.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CredmargRepository _repository;

        public EmployeeController(CredmargRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("create-employee")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _repository.Employees.Add(employee);
            return Ok();
        }

        [HttpGet("getall-employee")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmployees()
        {
            return Ok(_repository.Employees);
        }
    }
}
