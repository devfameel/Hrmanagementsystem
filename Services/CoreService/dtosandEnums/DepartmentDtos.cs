namespace CoreService.dtosandEnums
{
    public class DepartmentDtos
    {
    }

    public class DepartmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? DepartemntCode { get; set; } = string.Empty;

        public bool? IsDeleted { get; set; } = true;

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }
    }

    public class CreatedDepatmentDto
    {
        public string Name { get; set; }

        public string? DepartemntCode { get; set; } = string.Empty;

     //   public bool? IsDeleted { get; set; } = true;

        public DateTime? CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }

    }

}
