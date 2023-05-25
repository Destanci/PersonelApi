using Microsoft.EntityFrameworkCore;
using PersonelApi.Core.DataAccess;
using PersonelApi.Core.DataAccess.EntityFramework;
using PersonelApi.Core.Extensions;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.ComplexTypes;
using PersonelApi.Models.Entities;
using PersonelApi.Models.FilterModels;
using System.Linq.Expressions;

namespace PersonelApi.DataAccess
{
    public class EfEmployeeDal : EfEntityRepositoryBase<Employee>
    {
        public EfEmployeeDal(DbContext dbContext) : base(dbContext)
        {

        }

        public List<CtEmployee> GetDataList(Expression<Func<CtEmployee, bool>> filter, DataRequest<EmployeeFM> request)
        {
            var now = DateTime.Now;
            IQueryable<CtEmployee> data = dbContext.Set<Employee>()
                .Include(x => x.Department)
                .Include(x => x.Position)
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
                    Age = now.Year - x.BirthdayDate.Year,

                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department!.Name,
                    PositionId = x.PositionId,
                    PositionName = x.Position!.Name,
                })
                .Where(filter).GenerateDataOrder(request.Order);

            if(request.Start > 0) data = data.Skip(request.Start);
            if(request.Length > 0) data = data.Take(request.Length);
            return data.ToList();
        }

        public int CtCount(Expression<Func<CtEmployee, bool>> filter)
        {
            var now = DateTime.Now;
            return dbContext.Set<Employee>()
                .Include(x => x.Department)
                .Include(x => x.Position)
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
                    Age = now.Year - x.BirthdayDate.Year,

                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.Department!.Name,
                    PositionId = x.PositionId,
                    PositionName = x.Position!.Name,
                })
                .Where(filter)
                .Count();
        }
    }
}
