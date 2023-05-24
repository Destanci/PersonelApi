namespace PersonelApi.Models.FilterModels
{
    public class EmployeeFM
    {
        public EmployeeFM() {
            DepartmentIds = new HashSet<int>();
            PositionIds = new HashSet<int>();
        }
        public double MinAge {get; set;}
        public double MaxAge { get; set;}
        public string? Gender { get; set;}
        public ICollection<int> DepartmentIds { get; set; }
        public ICollection<int> PositionIds { get; set; }
    }
}
