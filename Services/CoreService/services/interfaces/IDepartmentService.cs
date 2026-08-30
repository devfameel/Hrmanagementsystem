using CoreService.dtosandEnums;

namespace CoreService.services.interfaces
{
    public interface IDepartmentService
    {
        Task<PaginationresponceDto<DepartmentDto>> GetAllDepatrmentAsync(int pageNumber, int pageSize , CancellationToken ct);

        Task<DepartmentDto> GetDepartmentbyIdAsync(int id, CancellationToken ct);

        Task<DepartmentDto> CreateDepatmentAsync(CreatedDepatmentDto input, CancellationToken ct);

        Task<DepartmentDto> UpdateDepatmentAsync(int id ,DepartmentDto input, CancellationToken ct);

        Task<bool> DeletedepartmentAsync(int id , CancellationToken ct);

        Task<List<DepartmentDto>> SearchdepatmentAync(string input, CancellationToken ct);

       // Task<List<DepartmentDto>> FilterDepartment(string input , CancellationToken ct);

    }
}
