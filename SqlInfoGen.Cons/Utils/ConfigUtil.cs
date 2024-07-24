using Microsoft.Extensions.Configuration;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Common;

namespace SqlInfoGen.Cons.Utils;

public static class ConfigUtil
{
    private static readonly IConfigurationRoot Root;
    private static readonly string Env;

    static ConfigUtil()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile($"{EnvUtil.GetCurrentWorkDirectory()}{ConfigCommon.ConfigFileName}", true, true);
        Root = builder.Build();
        Env = Root[ConfigCommon.EnvName];
    }

    public static string GetMySqlConnectionString()
    {
        return Root[$"{Env}:{ConfigCommon.ConnectionStringsName}"];
    }
    
    public static List<DbConfigPath> GetDbConfigPathList()
    {
        return Root.GetSection(ConfigCommon.ReadDbConfigPathsName).Get<List<DbConfigPath>>();
    }
}