using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Markdown.Title;

public sealed class MdH3(string content) : MdH(content)
{
    public override MdStructType Type => MdStructType.H3;
    protected override string PrefixIdentifier => "###";
}