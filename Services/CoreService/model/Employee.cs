namespace CoreService.model
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        public DateTime JoinDate { get; set; }

        public bool IsDeleted { get; set; } = true;


        public Department? Department { get; set; }
    }
}
