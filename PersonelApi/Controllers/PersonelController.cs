using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonelApi.Core.Extensions;
using PersonelApi.DataAccess;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.Entities;
using PersonelApi.Models.FilterModels;

namespace PersonelApi.Controllers
{
    [ApiController]
    public class PersonelController : ControllerBase
    {
        private readonly EfEmployeeDal _efEmployeeDal;

        public PersonelController(EfEmployeeDal efEmployeeDal)
        {
            _efEmployeeDal = efEmployeeDal;
        }

        [HttpPost]
        public ActionResult<string> GetPersonelList(DataRequest dataRequest, EmployeeFM filter)
        {
            if(!ModelState.IsValid)
            {
                return Forbid();
            }
            var now = DateTime.Now;

            var predicate = PredicateBuilder.New<Employee>(true);

            // Generate Filtering Predicate
            if (filter.MinAge > 0) predicate.And(x => x.BirthdayDate.GetDifferenceInYears(now) > filter.MinAge);
            if (filter.MaxAge > 0) predicate.And(x => x.BirthdayDate.GetDifferenceInYears(now) > filter.MaxAge);
            if(!string.IsNullOrWhiteSpace(filter.Gender)) predicate.And(x => x.Gender.Description() == filter.Gender);
            if (filter.DepartmentIds.Count > 0) predicate.And(x => filter.DepartmentIds.Contains(x.DepartmentId));
            if (filter.PositionIds.Count > 0) predicate.And(x => filter.PositionIds.Contains(x.PositionId));

            // Generate Searching Predicate
            if (!string.IsNullOrWhiteSpace(dataRequest.Search?.Value)) {
                var searchlist = dataRequest.Search?.Value?.Split(' ').ToList()!;
                predicate.And(x => searchlist.Contains(x.Name!));
                predicate.And(x => searchlist.Contains(x.Surname!));
                predicate.And(x => searchlist.Contains(x.Title!));
                predicate.And(x => searchlist.Contains(x.Phone!));
                predicate.And(x => searchlist.Contains(x.Email!));
            }

            var data = _efEmployeeDal.GetDataList(predicate, dataRequest!);

            var response = new DataResponse()
            {
                Data = data,
                Draw = dataRequest.Draw,
                RecordsFiltered = 0,
                RecordsTotal = 0,
            };

            return JsonConvert.SerializeObject(response);
        }
    }
}
