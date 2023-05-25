namespace PersonelApi.Models.ApiModels
{
    public class DataRequest<T> where T : class
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataRequestSearch? Search { get; set; }
        public ICollection<DataRequestOrder> Order { get; set; } = new HashSet<DataRequestOrder>();
        public T? Filter { get; set; }
    }

    public class DataRequestSearch
    {
        public string? Value { get; set; }
        public string? Regex { get; set; }
    }
    public class DataRequestOrder
    {
        public string? Column { get; set; }
        public string? Dir { get; set; }
    }
}
