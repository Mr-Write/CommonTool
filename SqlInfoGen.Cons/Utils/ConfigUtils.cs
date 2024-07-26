using Microsoft.Extensions.Configuration;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Bean.Config;
using SqlInfoGen.Cons.Common;

namespace SqlInfoGen.Cons.Utils;

public static class ConfigUtils
{
    private static readonly IConfigurationRoot Root;
    private static readonly string Env;

    static ConfigUtils()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile($"{EnvUtils.GetCurrentWorkDirectory()}{ConfigCommon.ConfigFileName}", true, true);
        Root = builder.Build();
        Env = Root[ConfigCommon.EnvName];
    }

    public static string GetMySqlConnectionString()
    {
        return Root[$"{Env}:{ConfigCommon.ConnectionStringsName}"];
    }
    
    public static List<DbConfigBean> GetDbConfigBeanList()
    {
        return Root.GetSection(ConfigCommon.ReadDbConfigPathsName).Get<List<DbConfigBean>>();
    }
}