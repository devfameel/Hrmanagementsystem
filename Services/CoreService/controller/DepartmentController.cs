using CoreService.dtosandEnums;
using CoreService.services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee([FromQuery] PaginationQuery query)
        {
            var Result = await _departmentService.GetAllDepatrmentAsync(query.pageNumber, query.pageSize, default);
            return Ok(Result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartmenteByIdAsync(int id)
        {
            var rressult = await _departmentService.GetDepartmentbyIdAsync(id, default);
            return Ok(rressult);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetDepartmentbySearchAsync([FromQuery] string input)
        {
            var result = await _departmentService.SearchdepatmentAync(input, default);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreatedDepatmentDto input)
        {
            var result = await _departmentService.CreateDepatmentAsync(input, default);
            return Ok(result);

        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmployeesync(int id, DepartmentDto input, CancellationToken ct = default)
        {
            var result = await _departmentService.UpdateDepatmentAsync(id, input, ct);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Employee Not Found"
                });
            }

            return Ok(result);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id, CancellationToken ct = default)
        {
            var result = await _departmentService.DeletedepartmentAsync(id, ct);

            if (result != true)
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
