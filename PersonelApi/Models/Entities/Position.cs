using PersonelApi.Core.DataAccess;

namespace PersonelApi.Models.Entities;

public partial class Position : IEntity
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Description { get; set; }

    public short? Status { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
