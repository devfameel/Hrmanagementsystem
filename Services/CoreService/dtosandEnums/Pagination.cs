namespace CoreService.dtosandEnums
{
    public class Pagination
    {
    }
    public class PaginationresponceDto<T>
    {
        public List<T?> items { get; set; }

        public int TotalCount { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }
    }
}
