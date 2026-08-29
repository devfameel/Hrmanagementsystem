using CoreService.dtosandEnums;

namespace CoreService.services.interfaces
{
    public interface IEmployeeService
    {
        Task<PaginationresponceDto<EmployeeDto>> GetAllEmployeeAsync(int pageNumber, int pageSize, CancellationToken ct);

        Task<EmployeeDto> GetEmployeebyIdAsync(int id , CancellationToken ct);

        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto inputEmployee , CancellationToken ct);

        Task<EmployeeDto> UpdateEmployeeAsync(int id , EmployeeDto inputEmployee , CancellationToken ct);

        Task<bool?> DeleteEmployeeAsync(int id , CancellationToken ct);

        Task<List<EmployeeDto>> SearchEmployeeAync (string input , CancellationToken ct);

    }
}
