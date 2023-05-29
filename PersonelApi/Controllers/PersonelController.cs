using LinqKit;
using Microsoft.AspNetCore.Mvc;
using PersonelApi.Core.Enums;
using PersonelApi.Core.Extensions;
using PersonelApi.DataAccess;
using PersonelApi.Models.ApiModels;
using PersonelApi.Models.ComplexTypes;
using PersonelApi.Models.Entities;
using PersonelApi.Models.FilterModels;

namespace PersonelApi.Controllers
{
    [ApiController]
    public class PersonelController : ControllerBase
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly EfEmployeeDal _efEmployeeDal;

        public PersonelController(IHostEnvironment hostEnvironment, EfEmployeeDal efEmployeeDal)
        {
            _efEmployeeDal = efEmployeeDal;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost, Route("api/[controller]/GetList", Name = "[controller]/GetList")]
        public IActionResult GetList([FromBody] DataRequest<EmployeeFM> dataRequest)
        {
            try
            {
                var now = DateTime.Now;

                var predicate = PredicateBuilder.New<CtEmployee>(true);

                // Generate dataRequest.filtering Predicate
                if (dataRequest.Filter != null)
                {
                    if (dataRequest.Filter.MinAge > 0) predicate.And(x => x.Age > dataRequest.Filter.MinAge);
                    if (dataRequest.Filter.MaxAge > 0) predicate.And(x => x.Age < dataRequest.Filter.MaxAge);
                    if (!string.IsNullOrWhiteSpace(dataRequest.Filter.Gender))
                    {
                        var name = dataRequest.Filter.Gender.ToPascalCase();
                        var isGender = Enum.TryParse(name, out Gender gender);
                        if(isGender) predicate.And(x => x.Gender! == gender);
                    }
                    if (dataRequest.Filter.DepartmentIds.Count > 0) predicate.And(x => dataRequest.Filter.DepartmentIds.Contains(x.DepartmentId));
                    if (dataRequest.Filter.PositionIds.Count > 0) predicate.And(x => dataRequest.Filter.PositionIds.Contains(x.PositionId));
                }

                // Generate Searching Predicate
                if (!string.IsNullOrWhiteSpace(dataRequest.Search?.Value))
                {
                    var searchPredicate = PredicateBuilder.New<CtEmployee>(true);
                    searchPredicate.Or(x => x.Name!.ToLower().Contains(dataRequest.Search!.Value.ToLower()));
                    searchPredicate.Or(x => x.Surname!.ToLower().Contains(dataRequest.Search!.Value.ToLower()));
                    searchPredicate.Or(x => x.Title!.ToLower().Contains(dataRequest.Search!.Value.ToLower()));
                    searchPredicate.Or(x => x.Phone!.ToLower().Contains(dataRequest.Search!.Value.ToLower()));
                    predicate.And(searchPredicate);
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
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/CreateList", Name = "[controller]/CreateList")]
        public IActionResult CreateList([FromBody] List<Employee> list)
        {
            try
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
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Success" } });
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/Create", Name = "[controller]/Create")]
        public IActionResult Create([FromBody] Employee item)
        {
            try
            {
                item.Id = 0;
                var result = _efEmployeeDal.Add(new Employee()
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
                if (result != null)
                {
                    return Ok(new Dictionary<String, dynamic>() {
                        { "Result", "Success" },
                        { "Id", result.Id }
                    });
                }
                else throw new Exception("Entity Create Failed");
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/Update", Name = "[controller]/Update")]
        public IActionResult Update([FromBody] Employee item)
        {
            try
            {
                if (!ModelState.IsValid)
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

                    var result = _efEmployeeDal.Update(entity);
                    return Ok(new Dictionary<String, dynamic>() {
                        { "Result", "Success" },
                        { "Id", result.Id }
                    });
                }
                else throw new Exception("Entity Update Failed");
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
                var entity = _efEmployeeDal.Get(x => x.Id == id);
                if (entity != null)
                {
                    _efEmployeeDal.Delete(entity);
                }

                return Ok(new Dictionary<String, dynamic>() { { "Result", "Success" } });
            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }

        [HttpPost, Route("api/[controller]/UploadImage", Name = "[controller]/UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] int id, IFormFile file)
        {
            try
            {
                var now = DateTime.Now;
                var employee = _efEmployeeDal.Get(x => x.Id == id) ?? throw new Exception("Employee Not Found");

                //file = HttpContext.Request.Form.Files.FirstOrDefault();

                if (file != null && file.Length > 0)
                {
                    string path = Path.GetFullPath(Path.Combine(_hostEnvironment.ContentRootPath, $"wwwroot/Employee/{employee.Id}"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fileName = $"profile_picture_{employee.Id}_{now:yyyy-MM-dd_HH-mm-ssfff}.jpg";
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);

                        employee.PicturePath = $"/Employee/{employee.Id}/{fileName}";

                        _efEmployeeDal.Update(employee);
                        fileStream.Close();
                    }
                    return Ok(new Dictionary<String, dynamic>() { { "Result", "Success" } });
                }
                else throw new Exception("No File Found");

            }
            catch (Exception ex)
            {
                return Ok(new Dictionary<String, dynamic>() { { "Result", "Failed: " + ex.Message } });
            }
        }
    }
}
