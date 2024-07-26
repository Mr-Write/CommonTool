using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public sealed class MdH5(string content) : MdH(content)
{
    public override MdStructType Type => MdStructType.H5;

    protected override string PrefixIdentifier => "#####";
}