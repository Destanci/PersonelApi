using LinqKit;
using Microsoft.AspNetCore.Mvc;
using PersonelApi.Core.Extensions;
using PersonelApi.DataAccess;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.ComplexTypes;
using PersonelApi.Models.Entities;
using PersonelApi.Models.FilterModels;

namespace PersonelApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PersonelController : ControllerBase
    {
        private readonly EfEmployeeDal _efEmployeeDal;

        public PersonelController(EfEmployeeDal efEmployeeDal)
        {
            _efEmployeeDal = efEmployeeDal;
        }

        [HttpPost, Route("GetPersonelList", Name = "GetPersonelList")]
        public IActionResult GetPersonelList([FromBody] DataRequest<EmployeeFM> dataRequest)
        {
            var now = DateTime.Now;

            var predicate = PredicateBuilder.New<CtEmployee>(true);

            // Generate dataRequest.filtering Predicate
            if (dataRequest.Filter != null)
            {
                if (dataRequest.Filter.MinAge > 0) predicate.And(x => x.Age > dataRequest.Filter.MinAge);
                if (dataRequest.Filter.MaxAge > 0) predicate.And(x => x.Age < dataRequest.Filter.MaxAge);
                if (!string.IsNullOrWhiteSpace(dataRequest.Filter.Gender)) predicate.And(x => x.Gender.Description() == dataRequest.Filter.Gender);
                if (dataRequest.Filter.DepartmentIds.Count > 0) predicate.And(x => dataRequest.Filter.DepartmentIds.Contains(x.DepartmentId));
                if (dataRequest.Filter.PositionIds.Count > 0) predicate.And(x => dataRequest.Filter.PositionIds.Contains(x.PositionId));
            }

            // Generate Searching Predicate
            if (!string.IsNullOrWhiteSpace(dataRequest.Search?.Value))
            {
                var searchlist = dataRequest.Search?.Value?.Split(' ').ToList()!;
                predicate.And(x => searchlist.Contains(x.Name!));
                predicate.And(x => searchlist.Contains(x.Surname!));
                predicate.And(x => searchlist.Contains(x.Title!));
                predicate.And(x => searchlist.Contains(x.Phone!));
                predicate.And(x => searchlist.Contains(x.Email!));
            }

            var data = _efEmployeeDal.GetDataList(predicate, dataRequest);

            var response = new DataResponse()
            {
                Data = data,
                Draw = dataRequest.Draw,
                RecordsFiltered = _efEmployeeDal.CtCount(predicate),
                RecordsTotal = _efEmployeeDal.Count(x => true),
            };

            return Ok(response);
        }

        [HttpPost, Route("CreateList", Name = "CreateList")]
        public IActionResult CreateList([FromBody] List<CtEmployee> list)
        {
            foreach (var item in list)
            {
                item.Id = 0;
                _efEmployeeDal.Add(new Employee()
                {
                    Name = item.Name,
                    Surname = item.Surname,
                    BirthdayDate = item.BirthdayDate,
                    Gender = item.Gender,
                    Email = item.Email,
                    Phone = item.Phone,
                    Address = item.Address,
                    DepartmentId = item.DepartmentId,
                    PositionId = item.PositionId,
                    Title = item.Title,
                    HireDate = item.HireDate,
                    About = item.About,
                    PicturePath = item.PicturePath,
                });
            }
            return Ok("Success");
        }

        [HttpPost, Route("Create", Name = "Create")]
        public IActionResult Create([FromBody] CtEmployee item)
        {
            item.Id = 0;
            _efEmployeeDal.Add(new Employee()
            {
                Name = item.Name,
                Surname = item.Surname,
                BirthdayDate = item.BirthdayDate,
                Gender = item.Gender,
                Email = item.Email,
                Phone = item.Phone,
                Address = item.Address,
                DepartmentId = item.DepartmentId,
                PositionId = item.PositionId,
                Title = item.Title,
                HireDate = item.HireDate,
                About = item.About
            });
            return Ok("Success");
        }

        [HttpPost, Route("Update", Name = "Update")]
        public IActionResult Update([FromBody] CtEmployee item)
        {
            if(!ModelState.IsValid)
            {
                return Forbid();
            }

            var entity = _efEmployeeDal.Get(x => x.Id == item.Id);
            if (entity != null)
            {
                entity.Name = item.Name;
                entity.Surname = item.Surname;
                entity.BirthdayDate = item.BirthdayDate; ;
                entity.Gender = item.Gender;
                entity.Email = item.Email;
                entity.Phone = item.Phone;
                entity.Address = item.Address;
                entity.DepartmentId = item.DepartmentId;
                entity.PositionId = item.PositionId;
                entity.Title = item.Title;
                entity.HireDate = item.HireDate;
                entity.About = item.About;

                _efEmployeeDal.Update(entity);
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
            var entity = _efEmployeeDal.Get(x => x.Id == id);
            if(entity != null)
            {
                _efEmployeeDal.Delete(entity);
            }

            return Ok("Success");
        }
    }
}
