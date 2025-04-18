using Hangfire;
using Shared;

namespace HangfireServer.Hangfires;

public class RecurringJobHandler
{
    private readonly IServiceProvider _serviceProvider;
    public RecurringJobHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 执行任务.
    /// </summary>
    /// <param name="taskRequest"></param>
    /// <returns>Task.</returns>
    public async Task Handler(TaskRequest taskRequest)
    {
        var ioc = _serviceProvider;

        var recurringJobManager = ioc.GetRequiredService<IRecurringJobManager>();
        var httpClientFactory = ioc.GetRequiredService<IHttpClientFactory>();
        var logger = ioc.GetRequiredService<ILogger<RecurringJobHandler>>();
        using var httpClient = httpClientFactory.CreateClient(taskRequest.ServiceName);

        // 无论是否请求成功，都算完成了本次任务
        try
        {
            // 请求子系统的接口
            var response = await httpClient.PostAsJsonAsync(taskRequest.ServiceName, taskRequest);

            var execteResult = await response.Content.ReadFromJsonAsync<ExecteTasResult>();

            // 被调用方要求取消任务
            if (execteResult != null && execteResult.CancelTask)
            {
                recurringJobManager.RemoveIfExists(taskRequest.TaskId);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Task error.");
        }
    }
}