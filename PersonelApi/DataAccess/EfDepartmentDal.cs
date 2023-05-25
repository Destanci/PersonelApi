using Microsoft.EntityFrameworkCore;
using PersonelApi.Core.DataAccess.EntityFramework;
using PersonelApi.Models.Entities;

namespace PersonelApi.DataAccess
{
    public class EfDepartmentDal : EfEntityRepositoryBase<Department>
    {
        public EfDepartmentDal(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
