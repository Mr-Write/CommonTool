namespace SqlInfoGen.Cons.Utils;

public static class GuidUtil
{
    public static string GetGuid()
    {
        return Guid.NewGuid().ToString().ToUpper().Replace("-", "");
    }
}