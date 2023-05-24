namespace PersonelApi.Models.ApiModels
{
    public class DataResponse
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public dynamic? Data { get; set; }
    }
}
