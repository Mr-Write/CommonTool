namespace SqlInfoGen.Cons.Bean;

public class DbConfigBean
{
    public string Table { get; set; }
    public bool NeedAllFields { get; set; } = true;
    public List<Field>? Fields { get; set; }
    public bool NeedComment { get; set; } = true;
    public List<string>? SelectConditions { get; set; }
    public List<string>? OrderByConditions { get; set; }
    public string OutputDir { get; set; } = "default";
    public string OutputFileName { get; set; } = "default";
    public string OutputFileSuffix { get; set; } = "md";
}

public class Field
{
    public string Name { get; set; }
    public string Alias { get; set; }
}