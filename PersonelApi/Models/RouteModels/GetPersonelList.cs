using PersonelApi.Models.ApiModels;
using PersonelApi.Models.FilterModels;
using System.ComponentModel.DataAnnotations;

namespace PersonelApi.Models.RouteModels
{
    public class GetPersonelList
    {
        [Required]
       // public DataRequest? dataRequest;
        public EmployeeFM? filter;
    }
}
