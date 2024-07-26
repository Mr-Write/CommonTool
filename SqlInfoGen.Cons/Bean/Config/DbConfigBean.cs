using SqlInfoGen.Cons.Enums;

namespace SqlInfoGen.Cons.Bean.Config;

/// <summary>
/// db 配置信息，关联 config.json 文件
/// </summary>
public class DbConfigBean
{
    /// <summary>
    /// 读取的文件配置层级
    /// <example>
    /// 以下配置表示的层级为：DbConfig/20212880/foxtian 文件夹层级
    /// <code lang="c#">
    /// var dbBean = new DbConfigBean();
    /// dbBean.ReadDirLevels = ["DbConfig", "20212880", "foxtian"];
    /// </code>
    /// </example>
    /// </summary>
    public List<string> ReadDirLevels { get; set; }

    /// <summary>
    /// 读取的配置文件名，且以 .json 结尾
    /// </summary>
    public string ReadFileName { get; set; }
    
    /// <summary>
    /// 无需在配置文件中配置，根据 ConnectionString 进行读取
    /// </summary>
    public string Db { get; set; }

    /// <summary>
    /// 输出文件夹
    /// </summary>
    public string OutputDir { get; set; } = "default";
    
    /// <summary>
    /// 输出文件名
    /// </summary>
    public string OutputFileName { get; set; } = "default";
    
    /// <summary>
    /// 输出文件的后缀
    /// </summary>
    public string OutputFileNameSuffix { get; set; } = "default";
    
    /// <summary>
    /// 输出文件后缀（格式）枚举
    /// </summary>
    public FileSuffixEnum OutputFileNameSuffixEnum { get; set; }

    /// <summary>
    /// 读取文件路径，根据 ReadDirLevels 与 ReadFileName 生成。
    /// </summary>
    public string ReadFilePath { get; set; } = null!;

    /// <summary>
    /// 输出文件路径
    /// </summary>
    public string OutputFilePath { get; set; } = null!;

    /// <summary>
    /// 连接字符串
    /// </summary>
    private string _connectionString = null!;
    
    /// <summary>
    /// 连接字符串，设置时会自动解析数据库名称
    /// </summary>
    public string ConnectionString
    {
        get => _connectionString;
        set
        {
            _connectionString = value;
            // 解析数据库名称
            Db = value.Split(';').First(x => x.ToLower().StartsWith("database=")).Split('=').Last();
        }
    }
}