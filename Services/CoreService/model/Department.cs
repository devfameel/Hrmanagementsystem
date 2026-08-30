namespace CoreService.model
{
    public class Department
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string DepartemntCode { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public DateTime? CreatedDate { get; set; }

        public int ? CreatedUserId { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();



    }
}
