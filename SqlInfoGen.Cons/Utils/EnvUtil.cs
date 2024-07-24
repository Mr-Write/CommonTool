namespace SqlInfoGen.Cons.Utils;

public static class EnvUtil
{
    /// <summary>
    /// 获取当前工作目录
    /// </summary>
    /// <param name="existsEndSeparator">最后是否携带分割符，默认 true 携带</param>
    /// <returns></returns>
    public static string GetCurrentWorkDirectory(bool existsEndSeparator = true)
    {
        return $"{Environment.CurrentDirectory}{(existsEndSeparator ? Path.DirectorySeparatorChar : "")}";
    }
    
}