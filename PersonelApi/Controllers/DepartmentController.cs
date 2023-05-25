using Microsoft.AspNetCore.Mvc;
using PersonelApi.DataAccess;
using PersonelApi.Models.ComplexTypes;
using PersonelApi.Models.Entities;

namespace PersonelApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DepartmentController : ControllerBase
    {
        private readonly EfDepartmentDal _efDepartmentDal;

        public DepartmentController(EfDepartmentDal efDepartmentDal)
        {
            _efDepartmentDal = efDepartmentDal;
        }

        [HttpPost, Route("GetList", Name = "GetList")]
        public IActionResult GetList()
        {
            var data = _efDepartmentDal.GetList(x => true);

            return Ok(data);
        }

        [HttpPost, Route("Create", Name = "Create")]
        public IActionResult Create([FromBody] Department item)
        {
            item.Id = 0;
            _efDepartmentDal.Add(new Department()
            {
                Name = item.Name,
            });
            return Ok("Success");
        }

        [HttpPost, Route("Update", Name = "Update")]
        public IActionResult Update([FromBody] Department item)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            var entity = _efDepartmentDal.Get(x => x.Id == item.Id);
            if (entity != null)
            {
                entity.Name = item.Name;

                _efDepartmentDal.Update(entity);
            }
            return Ok("Success");
        }

        [HttpPost, Route("Remove", Name = "Remove")]
        public IActionResult Remove([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }
            var entity = _efDepartmentDal.Get(x => x.Id == id);
            if (entity != null)
            {
                _efDepartmentDal.Delete(entity);
            }

            return Ok("Success");
        }
    }
}
