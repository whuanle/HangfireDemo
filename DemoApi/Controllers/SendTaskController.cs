using DemoApi.CQRS;
using DemoApi.Hangfires.Send;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers;

[ApiController]
[Route("/test")]
public class SendTaskController : ControllerBase
{
    private readonly SendHangfireService _hangfireService;

    public SendTaskController(SendHangfireService hangfireService)
    {
        _hangfireService = hangfireService;
    }

    [HttpGet("aaa")]
    public async Task<string> SendAsync()
    {
        await _hangfireService.Send(new MyTestRequest
        {
            CreateTime = DateTimeOffset.Now,
            CronExpression = "* * * * * *",
            TaskId = Guid.NewGuid().ToString(),
        });

        return "aaa";
    }
}
