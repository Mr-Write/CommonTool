using System.Text.Json;
using NUnit.Framework;
using SqlInfoGen.Cons.Utils;

namespace SqlInfoGen.Cons.Test;


public class SimpleTest : DbTest
{
    [Test]
    public void TestProgramPath1()
    {
        // 返回当前工作目录的路径
        string currentPath = Environment.CurrentDirectory;
        Console.WriteLine($"{currentPath}{Path.DirectorySeparatorChar}application.json");
    }

    [Test]
    public void TestDbConn()
    {
        Console.WriteLine(Db.CurrentConnectionConfig.ConnectionString);
    }

    [Test]
    public void TestReadStaticDir()
    {
        // 假设“Read”文件夹位于项目根目录
        string filePath = "ReadDbConfig/business.json";

        // 读取文件内容
        try
        {
            string content = File.ReadAllText(filePath);
            Console.WriteLine("文件内容:\n" + content);
        }
        catch (Exception ex)
        {
            Console.WriteLine("读取文件时出错: " + ex.Message);
        }
    }

    [Test]
    public void TestReadJsonObj()
    {
        var dbConfigPathList = ConfigUtil.GetDbConfigPathList();
        foreach (var readDbConfigPath in dbConfigPathList)
        {
            Console.WriteLine(readDbConfigPath.GetFilePath());
        }
    }

    [Test]
    public void TestGenBeans()
    {
        var dbConfigPathList = ConfigUtil.GetDbConfigPathList();
        foreach (var readDbConfigPath in dbConfigPathList)
        {
            var beans = JsonToObjUtil.GetBeanByDbConfigFile(readDbConfigPath.GetFilePath());
            // 打印 beans 信息
            Console.WriteLine(JsonSerializer.Serialize(beans));
        }
    }
}