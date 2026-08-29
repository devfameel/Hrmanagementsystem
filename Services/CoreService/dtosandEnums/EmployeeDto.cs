namespace CoreService.dtosandEnums
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public DateTime JoinDate { get; set; }

        public int? CreatedUserId { get; set; }

    }

    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public DateTime JoinDate { get; set; }

        public int? CreatedUserId { get; set; }

    }


}
