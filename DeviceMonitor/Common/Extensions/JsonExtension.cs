using Newtonsoft.Json;

namespace Common.Extensions;

public static class JsonExtension
{
    public static string SerializeToJson<T>(this T obj) where T: class?
    {
        string output = JsonConvert.SerializeObject(obj);
        return output;
    }
}