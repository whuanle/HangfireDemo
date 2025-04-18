using Hangfire.Redis.StackExchange;
using Hangfire;
using HangfireServer.Hangfires;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

ConfigureHangfire(builder.Services);

builder.Services.AddHttpLogging(o =>
{
    o.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;
    o.CombineLogs = true;
});

var app = builder.Build();
app.UseRouting();

app.MapControllers();
app.UseHangfireDashboard();

app.Run("http://0.0.0.0:5001");


void ConfigureHangfire(IServiceCollection services)
{
    var options =
        new RedisStorageOptions
        {
            // 配置 redis 前缀，每个任务实例都会创建一个 key
            Prefix = "aaa:aaa:hangfire",
        };

    services.AddHangfire(
        config =>
        {
            //config.UseRedisStorage("{redis连接字符串}", options)
            config.UseRedisStorage("127.0.0.1:6379", options)
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings();
            config.UseActivator(new HangfireActivator(services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()));
        });

    services.AddHangfireServer(options =>
    {
        // 注意，这里必须设置非常小的间隔
        options.SchedulePollingInterval = TimeSpan.FromSeconds(1);

        // 如果考虑到后续任务比较多，则需要调大此参数
        options.WorkerCount = 50;
    });
}