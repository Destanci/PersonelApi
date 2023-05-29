using Microsoft.AspNetCore.Mvc;
using PersonelApi.DataAccess;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.Entities;
using System;

namespace PersonelApi.Controllers
{
    [ApiController]
    public class PositionController : Controller
    {
        private readonly EfPositionDal _efPositionDal;

        public PositionController(EfPositionDal efPositionDal)
        {
            _efPositionDal = efPositionDal;
        }

        [HttpPost, Route("api/[controller]/GetList", Name = "[controller]/GetList")]
        public IActionResult GetList()
        {
            try
            {
                var data = _efPositionDal.GetList(x => true);
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
        public IActionResult Create([FromBody] Position item)
        {
            try
            {
                item.Id = 0;
                _efPositionDal.Add(new Position()
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
        public IActionResult Update([FromBody] Position item)
        {
            try
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
                var entity = _efPositionDal.Get(x => x.Id == id);
                if (entity != null)
                {
                    _efPositionDal.Delete(entity);
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
