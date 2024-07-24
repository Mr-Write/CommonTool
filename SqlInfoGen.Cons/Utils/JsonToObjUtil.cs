using System.Text.Json;
using SqlInfoGen.Cons.Bean;

namespace SqlInfoGen.Cons.Utils;

public static class JsonToObjUtil
{
    public static List<TableConfigBean> GetTableConfigBeanList(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        List<TableConfigBean> beans = JsonSerializer.Deserialize<List<TableConfigBean>>(jsonContent)!;
        return beans;
    }
}