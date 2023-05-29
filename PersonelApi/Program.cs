using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using PersonelApi.Core;
using PersonelApi.Core.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.Configure<ApplicationConfig>(builder.Configuration.GetSection("ApplicationConfig"));

builder.Services.ConfigureServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Proje", Version = "v1", Description = "rpje" });
    options.DescribeAllParametersInCamelCase();


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseStaticFiles(); 


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.DocumentTitle = "PersonelApp Rest Api";
});

app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.Run();
