using NUnit.Framework;
using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown;

public sealed class MdTable : MdStruct
{
    public override MdStructType Type => MdStructType.Table;

    /// <summary>
    /// 表头
    /// </summary>
    public List<string> Head { get; set; }

    /// <summary>
    /// 行数据
    /// </summary>
    public List<List<string>>? Rows { get; set; }
}