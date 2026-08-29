using CoreService.dtosandEnums;
using CoreService.services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CoreService.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Policy = "CanViewEmployee")]
        public async Task<IActionResult> GetEmployees(int PageNumber = 1 , int PageSixze = 15 ,CancellationToken ct = default)
        {
            var result = await _service.GetAllEmployeeAsync(PageNumber, PageSixze, ct);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "CanViewEmployee")]

        public async Task<IActionResult> GetemployeeByIdAsync(int id , CancellationToken cancellationToken = default)
        {
            var rressult = await _service.GetEmployeebyIdAsync(id, cancellationToken);

            return Ok(rressult);
        }

        [HttpGet("search")]
        [Authorize(Policy = "CanViewEmployee")]
        public async Task<IActionResult> GetEmployyBySearchAsync(string input , CancellationToken cancellationToken = default)
        {
            var result = await _service.SearchEmployeeAync(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost("create employee")]
        [Authorize(Policy = "CanCreateEmployee")]
        public async Task<IActionResult> CreateEmployeeAsync(CreateEmployeeDto input , CancellationToken ct = default)
        {
            var result = await _service.CreateEmployeeAsync(input, ct);
            return Ok(result);

        }

        [HttpPut("{id : int}")]
        [Authorize(Policy = "CanEditEmployee")]
        public async Task<IActionResult> UpdateEmployeesync(int id , EmployeeDto input , CancellationToken ct = default)
        {
            var result = await _service.UpdateEmployeeAsync(id,input, ct);

            if(result == null)
            {
                return NotFound(new
                {
                    message = "Employee Not Found"
                }); 
            }

            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "CanDeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id , CancellationToken ct = default)
        {
            var result = await _service.DeleteEmployeeAsync(id, ct);

            if (result != true && result == null)
            {
                return NotFound(new
                {
                    message = "Employee Not Found"
                });
            }

            return Ok(result);
        }
    }
}
