using CoreService.Data;
using CoreService.dtosandEnums;
using CoreService.model;
using CoreService.services.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client.NativeInterop;

namespace CoreService.services
{
    public class DepartmentService : IDepartmentService
    {

        private readonly CoreDbContext _context;

        public DepartmentService(CoreDbContext context)
        {
            _context = context;
        }
        public async Task<PaginationresponceDto<DepartmentDto>> GetAllDepatrmentAsync(int pageNumber, int pageSize, CancellationToken ct)
        {
            if(pageNumber < 1)
            {
                pageNumber = 1;
            }
            if(pageSize < 1)
            {
                pageSize = 15;
            }

            var Department =  _context.Departments.
               Where(x => !x.IsDeleted).AsNoTracking();

            var totalcount = await Department.CountAsync(ct);

            var dpet = await Department.OrderBy(x => x.Id).Skip((pageNumber-1) * pageSize)
                .Take(pageSize)
                .Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    DepartemntCode = x.DepartemntCode,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,

                }).ToListAsync(ct);

            var totalPages = (int)Math.Ceiling(
         totalcount / (double)pageSize);

            return new PaginationresponceDto<DepartmentDto>
            {
                CurrentPage = pageNumber,
                items = dpet,
                TotalCount = totalcount,
                TotalPage = totalPages
            };

        }

        public async Task<DepartmentDto> GetDepartmentbyIdAsync(int id, CancellationToken ct)
        {
            var dept = await _context.Departments.AsNoTracking().Where(x => x.Id == id && !x.IsDeleted).Select(x => new DepartmentDto
            {
                CreatedDate = x.CreatedDate,
                Name = x.Name,
                CreatedUserId = x.CreatedUserId ,
                DepartemntCode = x.DepartemntCode,
                Id = x.Id,
                IsDeleted = x.IsDeleted
               
            }).FirstOrDefaultAsync(ct);

            return dept;

        }

        public async Task<DepartmentDto> CreateDepatmentAsync(CreatedDepatmentDto input, CancellationToken ct)
        {
            var exixt = await _context.Departments.AnyAsync(x => x.DepartemntCode == input.DepartemntCode && !x.IsDeleted);

            if (exixt)
            {
                return null;
            }

            var dept = new Department
            {
                Name = input.Name,
                CreatedDate = input.CreatedDate,
                DepartemntCode = input.DepartemntCode,
                CreatedUserId = input.CreatedUserId,
                IsDeleted = false
            };

            _context.Departments.Add(dept);
            await _context.SaveChangesAsync(ct);

            return new DepartmentDto
            {
                Id = dept.Id,
                CreatedDate = dept.CreatedDate,
                CreatedUserId = dept.CreatedUserId,
                DepartemntCode = dept.DepartemntCode,
                Name = dept.Name,
                IsDeleted = dept.IsDeleted

            };
   
        }

        public async Task<DepartmentDto> UpdateDepatmentAsync(int id , DepartmentDto input, CancellationToken ct)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (department == null)
            {
                return null;
            }

            department.Name = input.Name;
            department.DepartemntCode = input.DepartemntCode;
            await _context.SaveChangesAsync(ct);

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                CreatedDate = department.CreatedDate,
                CreatedUserId = department.CreatedUserId,
                DepartemntCode = department.DepartemntCode,
                IsDeleted = department.IsDeleted,
            };

        }

        public async Task<bool> DeletedepartmentAsync(int id, CancellationToken ct)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (department == null)
            {
                return false;
            }

            department.IsDeleted = true;
            await _context.SaveChangesAsync(ct);
            return true;
        }
        public async Task<List<DepartmentDto>> SearchdepatmentAync(string input, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<DepartmentDto>();
            }

            var searchInput = input.Trim();

            var deprtment = await _context.Departments.AsNoTracking()
                .Where(x => !x.IsDeleted && (
                   x.Name.Contains(searchInput)
                   || x.DepartemntCode.Contains(searchInput)
                )).Select(x => new DepartmentDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DepartemntCode = x.DepartemntCode,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,
                    IsDeleted = x.IsDeleted,


                }).ToListAsync(ct);

            return deprtment;

        }
    }
}
