namespace Shared;

public class TaskRequest
{
    /// <summary>
    /// 任务 id.
    /// </summary>
    public string TaskId { get; set; } = "";

    /// <summary>
    /// 定时任务要请求的服务地址或服务名称.
    /// </summary>
    public string ServiceName { get; set; } = "";

    /// <summary>
    /// 参数类型名称.
    /// </summary>
    public string CommandType { get; set; } = "";

    /// <summary>
    /// 请求参数内容，json 序列化后的字符串.
    /// </summary>
    public string CommandBody { get; set; } = "";

    /// <summary>
    /// Cron 表达式.
    /// </summary>
    public string CronExpression { get; set; } = "";

    /// <summary>
    /// 创建时间.
    /// </summary>
    public string CreateTime { get; set; } = "";

}
