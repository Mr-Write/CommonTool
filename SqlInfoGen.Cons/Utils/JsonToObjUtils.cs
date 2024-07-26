using System.Text.Json;
using SqlInfoGen.Cons.Bean;

namespace SqlInfoGen.Cons.Utils;

public static class JsonToObjUtils
{
    public static List<T> GetTableConfigBeanList<T>(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        List<T> beans = JsonSerializer.Deserialize<List<T>>(jsonContent)!;
        return beans;
    }
}