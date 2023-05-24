using PersonelApi.Core.DataAccess;
using PersonelApi.Core.Enums;

namespace PersonelApi.Models.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime BirthdayDate { get; set; }
        public Gender Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public int DepartmentId { get; set; }
        public int PositionId { get; set; }

        public string? Title { get; set; }
        public DateTime? HireDate { get; set; }
        public string? About { get; set; }
        public string? PicturePath { get; set; }

        //public virtual Department { get; set; }
        //public virtual Position { get; set; }

    }
}
