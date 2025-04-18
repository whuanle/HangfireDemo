using Shared;

namespace DemoApi.Hangfires.Receive;

/// <summary>
/// 定义执行任务的抽象，便于忽略泛型处理.
/// </summary>
public interface IHangfireTaskHandler
{
    /// <summary>
    /// 执行任务.
    /// </summary>
    /// <param name="taskRequest"></param>
    /// <returns></returns>
    Task<ExecteTasResult> Handler(TaskRequest taskRequest);
}