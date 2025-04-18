using Maomi;
using Shared;
using System.Text.Json;
using System.Threading;

namespace DemoApi.Hangfires.Send;

/// <summary>
/// 定时任务服务，用于发送定时任务请求.
/// </summary>
[InjectOnScoped]
public class SendHangfireService
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    private readonly IHttpClientFactory _httpClientFactory;

    public SendHangfireService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// 发送定时任务请求.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TypeLoadException"></exception>
    public async Task Send<TCommand>(TCommand request)
        where TCommand : HangfireRequest
    {
        using var httpClient = _httpClientFactory.CreateClient();

        var taskRequest = new TaskRequest
        {
            TaskId = request.TaskId,
            CommandBody = JsonSerializer.Serialize(request, JsonOptions),
            ServiceName = "http://127.0.0.1:5000/hangfire/execute",
            CommandType = typeof(TCommand).Name ?? throw new TypeLoadException(typeof(TCommand).Name),
            CreateTime = request.CreateTime.ToUnixTimeMilliseconds().ToString(),
            CronExpression = request.CronExpression,
        };

        _ = await httpClient.PostAsJsonAsync("http://127.0.0.1:5001/execute/addtask", taskRequest);
    }

    /// <summary>
    /// 取消定时任务.
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public async Task Cancel(string taskId)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        _ = await httpClient.PostAsJsonAsync("http://127.0.0.1:5001/hangfire/cancel", new CancelTaskRequest
        {
            TaskId = taskId
        });

    }
}
