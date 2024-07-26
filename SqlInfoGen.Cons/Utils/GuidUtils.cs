namespace SqlInfoGen.Cons.Utils;

public static class GuidUtils
{
    public static string GetGuid()
    {
        return Guid.NewGuid().ToString().ToUpper().Replace("-", "");
    }
}