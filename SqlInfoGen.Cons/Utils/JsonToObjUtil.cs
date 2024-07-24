using System.Text.Json;
using SqlInfoGen.Cons.Bean;

namespace SqlInfoGen.Cons.Utils;

public static class JsonToObjUtil
{
    public static List<DbConfigBean> GetBeanByDbConfigFile(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        List<DbConfigBean> beans = JsonSerializer.Deserialize<List<DbConfigBean>>(jsonContent)!;
        return beans;
    }
}