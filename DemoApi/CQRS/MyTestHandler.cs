using MediatR;
using Shared;

namespace DemoApi.CQRS;

/// <summary>
/// 要被定时任务执行的代码.
/// </summary>
public class MyTestHandler : IRequestHandler<MyTestRequest, ExecteTasResult>
{
    private static volatile int _count;
    private static DateTimeOffset _lastTime;

    public async Task<ExecteTasResult> Handle(MyTestRequest request, CancellationToken cancellationToken)
    {
        _count++;
        if (_lastTime == default)
        {
            _lastTime = DateTimeOffset.Now;
        }

        Console.WriteLine($"""
            执行时间：{DateTimeOffset.Now.ToString("HH:mm:ss.ffff")}
            执行频率(每 10s)：{(_count / (DateTimeOffset.Now - _lastTime).TotalSeconds * 10)}
            """);

        return new ExecteTasResult
        {
            CancelTask = false
        };
    }
}