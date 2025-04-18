using Hangfire;
using HangfireServer.Hangfires;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace HangfireServer.Controllers;

[ApiController]
[Route("/execute")]
public class HangfireController : ControllerBase
{
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireController(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    [HttpPost("addtask")]
    public async Task<TaskResponse> AddTask(TaskRequest value)
    {
        await Task.CompletedTask;
        _recurringJobManager.AddOrUpdate<RecurringJobHandler>(
            value.TaskId,
            task => task.Handler(value),
            cronExpression: value.CronExpression,
            options: new RecurringJobOptions
            {
            });
        return new TaskResponse {  };
    }

    [HttpPost("cancel")]
    public async Task<TaskResponse> Cancel(CancelTaskRequest value)
    {
        await Task.CompletedTask;
        _recurringJobManager.RemoveIfExists(value.TaskId);

        return new TaskResponse
        {
        };
    }
}
