using Microsoft.Extensions.Configuration;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Bean.Config;
using SqlInfoGen.Cons.Common;

namespace SqlInfoGen.Cons.Utils;

public static class ConfigUtils
{
    private static readonly IConfigurationRoot Root;

    static ConfigUtils()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile($"{EnvUtils.GetCurrentWorkDirectory()}{ConfigCommon.ConfigFileName}", true, true);
        Root = builder.Build();
    }
    
    public static List<DbConfigBean> GetDbConfigBeanList()
    {
        return Root.GetSection(ConfigCommon.ReadDbConfigPathsName).Get<List<DbConfigBean>>();
    }
}