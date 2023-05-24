using PersonelApi.Core.Enums;

namespace PersonelApi.Models.ComplexTypes
{
    public class CtEmployee
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
        public string? DepartmentName { get; set; }
        public int PositionId { get; set; }
        public string? PositionName { get; set; }

        public string? Title { get; set; }
        public DateTime? HireDate { get; set; }
        public string? About { get; set; }
        public string? PicturePath { get; set; }

    }
}
