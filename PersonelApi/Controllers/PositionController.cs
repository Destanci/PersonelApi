using Microsoft.AspNetCore.Mvc;
using PersonelApi.DataAccess;
using PersonelApi.Models.Entities;

namespace PersonelApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PositionController : Controller
    {
        private readonly EfPositionDal _efPositionDal;

        public PositionController(EfPositionDal efPositionDal)
        {
            _efPositionDal = efPositionDal;
        }

        [HttpPost, Route("GetList", Name = "GetList")]
        public IActionResult GetList()
        {
            var data = _efPositionDal.GetList(x => true);

            return Ok(data);
        }

        [HttpPost, Route("Create", Name = "Create")]
        public IActionResult Create([FromBody] Position item)
        {
            item.Id = 0;
            _efPositionDal.Add(new Position()
            {
                Name = item.Name,
            });
            return Ok("Success");
        }

        [HttpPost, Route("Update", Name = "Update")]
        public IActionResult Update([FromBody] Position item)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            var entity = _efPositionDal.Get(x => x.Id == item.Id);
            if (entity != null)
            {
                entity.Name = item.Name;

                _efPositionDal.Update(entity);
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
            var entity = _efPositionDal.Get(x => x.Id == id);
            if (entity != null)
            {
                _efPositionDal.Delete(entity);
            }

            return Ok("Success");
        }
    }
}
