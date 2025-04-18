using DemoApi;
using Maomi;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddModule<ApiModule>();

builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;
    o.CombineLogs = true;
});

var app = builder.Build();

app.UseRouting();

app.UseHttpLogging();

app.MapControllers();

app.Run("http://0.0.0.0:5000");
