using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean;

public class TableConfigBean
{
    public string Table { get; set; }
    public bool NeedAllFields { get; set; } = true;
    public List<Field>? Fields { get; set; }
    public bool NeedTableSchema { get; set; } = true;
    public List<string>? SelectConditions { get; set; }
    public List<string>? OrderByConditions { get; set; }
    public Limit Limit { get; set; }
}

public class Field
{
    public string Name { get; set; }
    public string? Alias { get; set; }
}

public class Limit
{
    /// <summary>
    /// 偏移量
    /// </summary>
    public int Offset { get; set; } = 0;
    
    /// <summary>
    /// 获取数量
    /// </summary>
    public int Count { get; set; } = 10;
}