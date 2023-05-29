using Microsoft.AspNetCore.Mvc;
using PersonelApi.DataAccess;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.ComplexTypes;
using PersonelApi.Models.Entities;

namespace PersonelApi.Controllers
{
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly EfDepartmentDal _efDepartmentDal;

        public DepartmentController(EfDepartmentDal efDepartmentDal)
        {
            _efDepartmentDal = efDepartmentDal;
        }

        [HttpPost, Route("api/[controller]/GetList", Name = "[controller]/GetList")]
        public IActionResult GetList()
        {
            try
            {
                var data = _efDepartmentDal.GetList(x => true);
                var count = data.Count();
                var response = new DataResponse()
                {
                    Draw = 0,
                    Data = data,
                    RecordsFiltered = count,
                    RecordsTotal = count,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/Create", Name = "[controller]/Create")]
        public IActionResult Create([FromBody] Department item)
        {
            try
            {
                item.Id = 0;
                _efDepartmentDal.Add(new Department()
                {
                    Name = item.Name,
                });
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Success" } });
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/Update", Name = "[controller]/Update")]
        public IActionResult Update([FromBody] Department item)
        {
            try
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
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Success" } });
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/Remove", Name = "[controller]/Remove")]
        public IActionResult Remove([FromBody] int id)
        {
            try
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

                return Ok(new Dictionary<String, dynamic>() { { "Result", "Success" } });
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }
    }
}
