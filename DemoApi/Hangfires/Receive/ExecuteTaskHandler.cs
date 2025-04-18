using MediatR;
using Shared;
using System.Text.Json;

namespace DemoApi.Hangfires.Receive;

/// <summary>
/// 用于反序列化参数并发送 Command.
/// </summary>
/// <typeparam name="TCommand">命令.</typeparam>
public class ExecuteTaskHandler<TCommand> : IHangfireTaskHandler
    where TCommand : HangfireRequest, IRequest<ExecteTasResult>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecuteTaskHandler{TCommand}"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public ExecuteTaskHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    /// <inheritdoc/>
    public async Task<ExecteTasResult> Handler(TaskRequest taskRequest)
    {
        var command = JsonSerializer.Deserialize<TCommand>(taskRequest.CommandBody, JsonSerializerOptions)!;
        if (command == null)
        {
            throw new Exception("解析命令参数失败");
        }

        // 处理命令的逻辑
        var response = await _mediator.Send(command);
        return response;
    }
}
