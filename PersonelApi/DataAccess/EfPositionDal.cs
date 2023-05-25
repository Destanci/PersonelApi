using Microsoft.EntityFrameworkCore;
using PersonelApi.Core.DataAccess.EntityFramework;
using PersonelApi.Models.Entities;

namespace PersonelApi.DataAccess
{
    public class EfPositionDal : EfEntityRepositoryBase<Position>
    {
        public EfPositionDal(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
