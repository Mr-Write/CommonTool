namespace SqlInfoGen.Cons.Common;

public static class ConfigCommon
{
    public static readonly string ConfigFileName = "config.json";
    public static readonly string ReadDbConfigPathsName = "ReadDbConfigPaths";

    public static readonly string DefaultOutputDir =
        $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Path.DirectorySeparatorChar}TableGen";

    public static string DefaultOutputFileNamePrefix => $"{DateTime.Now:yyyyMMddHHmmssfff}_";
    public static readonly string DefaultOutputFileNameSuffix = "md";

    public static readonly string DefaultName = "default";
}