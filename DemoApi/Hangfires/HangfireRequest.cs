using MediatR;
using Shared;

namespace DemoApi.Hangfires;

/// <summary>
/// 定时任务抽象参数，也可以改成特性注解.
/// </summary>
public abstract class HangfireRequest : IRequest<ExecteTasResult>
{
    /// <summary>
    /// 定时任务 id.
    /// </summary>
    public string TaskId { get; init; } = string.Empty;

    /// <summary>
    /// 定时任务表达式.
    /// </summary>
    public string CronExpression { get; init; } = string.Empty;

    /// <summary>
    /// 该任务创建时间.
    /// </summary>
    public DateTimeOffset CreateTime { get; init; }
}
