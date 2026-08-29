using Azure.Core;
using CoreService.Data;
using CoreService.dtosandEnums;
using CoreService.model;
using CoreService.services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreService.services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly CoreDbContext _context;

        public EmployeeService(CoreDbContext context)
        {
            _context = context;

        }

        public async Task<PaginationresponceDto<EmployeeDto>> GetAllEmployeeAsync(int pageNumber, int pageSize , CancellationToken ct)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 15;

            }
            var query = _context.Employees.Where(x => !x.IsDeleted).AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var employee = await query.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).
                Select(x => new EmployeeDto
                {

                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DepartmentId = x.DepartmentId,
                    JoinDate = x.JoinDate,
                    CreatedUserId = x.CreatedUserId,
                    Department = x.Department

                }).ToListAsync(ct);

            var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

            return new PaginationresponceDto<EmployeeDto>
            {
                items = employee,
                CurrentPage = pageNumber,
                TotalCount = totalCount,
                TotalPage = totalPages,
            };
        }

        public async Task<EmployeeDto> GetEmployeebyIdAsync(int id , CancellationToken ct)
        {
            var employee = await _context.Employees.AsNoTracking().Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    FirstName= x.FirstName,
                    LastName = x.LastName,
                    JoinDate= x.JoinDate,
                    DepartmentId= x.DepartmentId,
                    Department = x.Department,
                    CreatedUserId = x.CreatedUserId,
                    Email = x.Email,
                      
                }).FirstOrDefaultAsync(ct);

            return employee;
                
        }
        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto inputEmployee , CancellationToken ct)
        {
            var emailExist = await _context.Employees.AnyAsync(x => x.Email == inputEmployee.Email);

            if(emailExist)
            {
                return null;
            }

            var employee = new Employee
            {
                FirstName = inputEmployee.FirstName,
                LastName = inputEmployee.LastName,
                DepartmentId = inputEmployee.DepartmentId,
                Email = inputEmployee.Email,
                CreatedUserId = inputEmployee.CreatedUserId,
                JoinDate = DateTime.Now,
                IsDeleted = false 
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync(ct);
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                CreatedUserId = employee.CreatedUserId,
                Department = employee.Department,
                DepartmentId = employee.DepartmentId,
                Email = employee.Email,
                JoinDate = employee.JoinDate,
            };
        }
        public async Task<EmployeeDto> UpdateEmployeeAsync(int id , EmployeeDto inputEmployee, CancellationToken ct)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if(employee == null)
            {
                return null;
            }
            var emailExistsForOtherUser = await _context.Employees.AnyAsync(x => x.Email == inputEmployee.Email && x.Id != id);

            if(emailExistsForOtherUser)
            {
                return null;
            }

            employee.FirstName = inputEmployee.FirstName;
            employee.LastName = inputEmployee.LastName;
            employee.Email = inputEmployee.Email;
            employee.DepartmentId = inputEmployee.DepartmentId;

            await _context.SaveChangesAsync(ct);

            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                JoinDate = employee.JoinDate,
                CreatedUserId = employee.CreatedUserId,
                Email = employee.Email,
                DepartmentId = employee.DepartmentId,
                Department = employee.Department
            };

        }

        public async Task<bool?> DeleteEmployeeAsync(int id, CancellationToken ct)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if(employee == null)
            {
                return false;
            }

            employee.IsDeleted = true;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<List<EmployeeDto>> SearchEmployeeAync(string input , CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<EmployeeDto>();
            }

            var searchInput = input.Trim();

            var employee = await _context.Employees.AsNoTracking()
                .Where(x => !x.IsDeleted && (
                  x.FirstName.Contains(searchInput)
                  ||x.LastName.Contains(searchInput)
                  || x.Email.Contains(searchInput))
                ).Select(e => new EmployeeDto
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    
                    CreatedUserId = e.CreatedUserId,
                    Department = e.Department,
                    DepartmentId = e.DepartmentId,
                    Id = e.Id,
                    JoinDate = e.JoinDate,
                    
                    
                }).ToListAsync(ct);

            return employee;
        }

    }
}
