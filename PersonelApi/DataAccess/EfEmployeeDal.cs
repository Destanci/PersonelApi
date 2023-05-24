using Microsoft.EntityFrameworkCore;
using PersonelApi.Core.DataAccess.EntityFramework;
using PersonelApi.Core.Extensions;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.ComplexTypes;
using PersonelApi.Models.Entities;
using System.Linq.Expressions;

namespace PersonelApi.DataAccess
{
    public class EfEmployeeDal : EfEntityRepositoryBase<Employee>
    {
        public EfEmployeeDal(DbContext dbContext) : base(dbContext)
        {

        }

        public List<CtEmployee> GetDataList(Expression<Func<Employee, bool>> filter, DataRequest request)
        {
            return dbContext.Set<Employee>()
                .Where(filter)
                .Select(x => new CtEmployee()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    About = x.About,
                    Address = x.Address,
                    BirthdayDate = x.BirthdayDate,
                    Email = x.Email,
                    Gender = x.Gender,
                    HireDate = x.HireDate,
                    Phone = x.Phone,
                    PicturePath = x.PicturePath,
                    Title = x.Title,

                    DepartmentId = x.DepartmentId,
                    DepartmentName = "",
                    PositionId = x.PositionId,
                    PositionName = "",
                })
                .GenerateDataOrder(request.Order)
                .Skip(request.Start)
                .Take(request.Length)
                .ToList();
        }
    }
}
