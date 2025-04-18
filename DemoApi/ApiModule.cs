using DemoApi.Hangfires;
using DemoApi.Hangfires.Receive;
using Maomi;

namespace DemoApi;

public class ApiModule : Maomi.ModuleCore, IModule
{
    private readonly HangireTypeFactory _hangireTypeFactory;

    public ApiModule()
    {
        _hangireTypeFactory = new HangireTypeFactory();
    }

    public override void ConfigureServices(ServiceContext context)
    {
        context.Services.AddTransient(typeof(ExecuteTaskHandler<>));
        context.Services.AddSingleton(_hangireTypeFactory);
        context.Services.AddHttpClient();
        context.Services.AddMediatR(o =>
        {
            o.RegisterServicesFromAssemblies(context.Modules.Select(x => x.Assembly).ToArray());
        });
    }

    public override void TypeFilter(Type type)
    {
        if (!type.IsClass || type.IsAbstract)
        {
            return;
        }

        if (type.IsAssignableTo(typeof(HangfireRequest)))
        {
            _hangireTypeFactory.Add(type);
        }
    }
}