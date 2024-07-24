using Microsoft.Extensions.Configuration;

namespace SqlInfoGen.Cons.Utils;

public static class ConfigUtil
{
    private static readonly IConfigurationRoot Root;
    private static readonly string Env;

    static ConfigUtil()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile($"{EnvUtil.GetCurrentWorkDirectory()}application.json", true, true);
        Root = builder.Build();
        Env = Root["env"];
    }

    public static string GetMySqlConnectionString()
    {
        return Root[$"{Env}:connectionStrings"];
    }
}