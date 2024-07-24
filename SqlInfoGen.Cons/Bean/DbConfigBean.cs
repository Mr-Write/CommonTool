using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean;

public class DbConfigBean
{
    public List<string> ReadDirLevels { get; set; }

    public string ReadFileName { get; set; }
    public string Db { get; set; }

    public string OutputDir { get; set; } = "default";
    public string OutputFileName { get; set; } = "default";
    public string OutputFileNameSuffix { get; set; } = "default";
    public FileSuffixEnum OutputFileNameSuffixEnum { get; set; }
    
    public string ReadFilePath;
    public string OutputFilePath;
}