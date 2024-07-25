namespace SqlInfoGen.Cons.Common;

public static class ConfigCommon
{
    public static readonly string ConfigFileName = "config.json";
    public static readonly string EnvName = "env";
    public static readonly string ConnectionStringsName = "connectionStrings";
    public static readonly string ReadDbConfigPathsName = "readDbConfigPaths";
    
    public static readonly string DefaultOutputDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Path.DirectorySeparatorChar}TableGen";
    public static string DefaultOutputFileNamePrefix => $"{DateTime.Now:yyyyMMddHHmmss}_";
    public static readonly string  DefaultOutputFileNameSuffix = "md";
    
    public static readonly string  DefaultName = "default";
    
}