using DemoApi.Hangfires;
using MediatR;
using Shared;

namespace DemoApi.CQRS;

public class MyTestRequest : HangfireRequest, IRequest<ExecteTasResult>
{
}
