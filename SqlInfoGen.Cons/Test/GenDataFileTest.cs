using System.Text.Json;
using NUnit.Framework;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Utils;

namespace SqlInfoGen.Cons.Test;

public class GenDataFileTest : DbTest
{
    [Test]
    public void TestGenDataFile()
    {
        var dbConfigPathList = ConfigUtil.GetDbConfigPathList();
        foreach (var readDbConfigPath in dbConfigPathList)
        {
            var beans = JsonToObjUtil.GetBeanByDbConfigFile(readDbConfigPath.GetFilePath());
            foreach (var bean in beans)
            {
                Console.WriteLine(JsonSerializer.Serialize(bean));
                // 检查是否遵循规则
                var errorInfo = CheckFollowRule(bean);
                if (errorInfo is not null)
                {
                    Console.WriteLine($"{readDbConfigPath.GetFilePath()} 中存在错误信息: {errorInfo}");
                    continue;
                }
                
            }
        }
    }

    private static string? CheckFollowRule(DbConfigBean bean)
    {
        if (string.IsNullOrWhiteSpace(bean.Table))
        {
            return "Table 不能为空";
        }

        return null;
    }
}