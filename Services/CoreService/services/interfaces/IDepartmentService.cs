using CoreService.dtosandEnums;

namespace CoreService.services.interfaces
{
    public interface IDepartmentService
    {
        Task<PaginationresponceDto<DepartmentDto>> GetAllDepatrmentAsync(int pageNumber, int pageSize);

        Task<DepartmentDto> GetDepartmentbyIdAsync(int id);

        Task<DepartmentDto> CreateDepatmentAsync(CreatedDepatmentDto input);

        Task<DepartmentDto> UpdateDepatmentAsync(DepartmentDto input);

        Task<bool> DeletedepartmentAsync(int id);

        Task<List<DepartmentDto>> SearchdepatmentAync(string input);

        Task<List<DepartmentDto>> FilterDepartment(string input);

    }
}
