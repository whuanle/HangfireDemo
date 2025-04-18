using DemoApi.Hangfires;
using DemoApi.Hangfires.Receive;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace DemoApi.Controllers;

/// <summary>
/// 定时任务触发入口.
/// </summary>
[ApiController]
[Route("/hangfire")]
public class ExecuteController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HangireTypeFactory _hangireTypeFactory;

    public ExecuteController(IServiceProvider serviceProvider, HangireTypeFactory hangireTypeFactory)
    {
        _serviceProvider = serviceProvider;
        _hangireTypeFactory = hangireTypeFactory;
    }

    [HttpPost("execute")]
    public async Task<ExecteTasResult> ExecuteTask([FromBody] TaskRequest request)
    {
        var commandType = _hangireTypeFactory.Get(request.CommandType);

        // 找不到该事件类型，取消后续事件执行
        if (commandType == null)
        {
            return new ExecteTasResult
            {
                CancelTask = true
            };
        }

        var commandTypeHandler = typeof(ExecuteTaskHandler<>).MakeGenericType(commandType);

        var handler = _serviceProvider.GetService(commandTypeHandler) as IHangfireTaskHandler;
        if(handler == null)
        {
            return new ExecteTasResult
            {
                CancelTask = true
            };
        }

        return await handler.Handler(request);
    }
}
