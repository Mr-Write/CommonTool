namespace SqlInfoGen.Cons.Bean;

public class DbConfigPath
{
    public List<string> DirLevels { get; set; }

    public string FileName { get; set; }

    public string GetFilePath() =>
        Path.Combine(Path.Combine(DirLevels.ToArray()), FileName);
}