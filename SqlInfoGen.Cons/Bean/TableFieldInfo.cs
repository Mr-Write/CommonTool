namespace SqlInfoGen.Cons.Bean;

public class TableFieldInfo
{
    public string Field { get; set; } = null!;
    public string Type { get; set; } = null!;

    /// <summary>
    /// 当创建表时没有 Comment，默认给 ''
    /// </summary>
    public string Comment { get; set; } = null!;
    public int Order { get; set; }
}