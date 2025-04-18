using System.Collections.Concurrent;

namespace DemoApi.Hangfires;

/// <summary>
/// 记录 CQRS 中的命令类型，以便能够通过字符串快速查找 Type.
/// </summary>
public class HangireTypeFactory
{
    private readonly ConcurrentDictionary<string, Type> _typeDictionary;
    public HangireTypeFactory()
    {
        _typeDictionary = new ConcurrentDictionary<string, Type>();
    }

    public void Add(Type type)
    {
        if (!_typeDictionary.ContainsKey(type.Name))
        {
            _typeDictionary[type.Name] = type;
        }
    }

    public Type? Get(string typeName)
    {
        if (_typeDictionary.TryGetValue(typeName, out var type))
        {
            return type;
        }

        return _typeDictionary.FirstOrDefault(x => x.Value.FullName == typeName).Value;
    }
}
