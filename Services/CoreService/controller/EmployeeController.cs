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
        public async Task<IActionResult> GetEmployees([FromQuery] PaginationQuery query)
        {
            var result = await _service.GetAllEmployeeAsync(query.pageNumber, query.pageSize, default);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetemployeeByIdAsync(int id)
        {
            var rressult = await _service.GetEmployeebyIdAsync(id, default);
            return Ok(rressult);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetEmployyBySearchAsync([FromQuery] string input)
        {
            var result = await _service.SearchEmployeeAync(input, default);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeDto input)
        {
            var result = await _service.CreateEmployeeAsync(input, default);
            return Ok(result);

        }

        [HttpPut("{id:int}")]
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
