using NUnit.Framework;

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
}