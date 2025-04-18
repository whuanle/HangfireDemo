﻿using Hangfire;
using Hangfire.Redis.StackExchange;
using System.Diagnostics.CodeAnalysis;

namespace HangfireServer.Hangfires;

/// <summary>
/// 任务容器.
/// </summary>
public class HangfireJobActivatorScope : JobActivatorScope
{
    private readonly IServiceScope _serviceScope;
    private readonly string _jobId;

    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireJobActivatorScope"/> class.
    /// </summary>
    /// <param name="serviceScope"></param>
    /// <param name="jobId"></param>
    public HangfireJobActivatorScope([NotNull] IServiceScope serviceScope, string jobId)
    {
        _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
        _jobId = jobId;
    }

    /// <inheritdoc/>
    public override object Resolve(Type type)
    {
        var res = ActivatorUtilities.GetServiceOrCreateInstance(_serviceScope.ServiceProvider, type);
        return res;
    }

    /// <inheritdoc/>
    public override void DisposeScope()
    {
        _serviceScope.Dispose();
    }
}

