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


        [HttpGet]
        public ActionResult<string> Hi()
        {
            return "Hello";
        }

        [HttpPost]
        public IActionResult GetPersonelList([FromBody] DataRequest<EmployeeFM> dataRequest)
        {
            //if (!ModelState.IsValid)
            //{
            //    return "";
            //}
            var now = DateTime.Now;

            var predicate = PredicateBuilder.New<CtEmployee>(true);

            // Generate dataRequest.filtering Predicate
            if (dataRequest.filter != null)
            {
                if (dataRequest.filter.MinAge > 0) predicate.And(x => x.Age > dataRequest.filter.MinAge);
                if (dataRequest.filter.MaxAge > 0) predicate.And(x => x.Age < dataRequest.filter.MaxAge);
                if (!string.IsNullOrWhiteSpace(dataRequest.filter.Gender)) predicate.And(x => x.Gender.Description() == dataRequest.filter.Gender);
                if (dataRequest.filter.DepartmentIds.Count > 0) predicate.And(x => dataRequest.filter.DepartmentIds.Contains(x.DepartmentId));
                if (dataRequest.filter.PositionIds.Count > 0) predicate.And(x => dataRequest.filter.PositionIds.Contains(x.PositionId));
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

        // [HttpPost]
        [HttpPost, Route("AddPersonel", Name = "AddPersonel")]
        public ActionResult<string> AddPersonel([FromBody] List<CtEmployee> list)
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
            return "Success";
        }
    }
}
