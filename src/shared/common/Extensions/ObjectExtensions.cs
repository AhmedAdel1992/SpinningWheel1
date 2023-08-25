using System.Text.Json;

namespace Common.Extensions;

public static class ObjectExtensions
{
    public static string Serialize(this object src)
    {
        return JsonSerializer.Serialize(src);
    }
}