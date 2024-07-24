using System.Text;
using System.Text.Json;
using NUnit.Framework;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Helpers;
using SqlInfoGen.Cons.Utils;

namespace SqlInfoGen.Cons.Test;

public class GenDataFileTest : DbTest
{
    
    [Test]
    public void TestGenDataFile()
    {
        var dbConfigPathList = ConfigUtil.GetDbConfigPathList();
        foreach (var readDbConfigPath in dbConfigPathList)
        {
            var beans = JsonToObjUtil.GetBeanByDbConfigFile(readDbConfigPath.GetFilePath());
            foreach (var bean in beans)
            {
                Console.WriteLine(JsonSerializer.Serialize(bean));
                var tableFieldInfos = DbHelper.GetTableSchema(bean.Table);
                // 检查是否遵循规则
                var errorInfo = CheckFollowRule(bean, tableFieldInfos);
                if (errorInfo is not null)
                {
                    Console.WriteLine($"{readDbConfigPath.GetFilePath()} 中存在错误信息: {errorInfo}");
                    continue;
                }
                // 生成 sql
                var sql = GenSql(bean, tableFieldInfos);
                
                // 执行 sql
                var dataTable = DbHelper.GetDataTable(sql);
            }
        }
    }

    private static string GenSql(DbConfigBean bean, Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        var builder = new StringBuilder();
        builder.Append("select ");
        // 配置查询的字段
        builder.Append(bean.NeedAllFields
            ? string.Join(", ", tableFieldInfos.Values.OrderBy(f => f.Order).Select(f => f.Field))
            : string.Join(",", bean.Fields!));
        // 配置查询的表
        builder.Append($" from {bean.Table} ");
        // 配置查询条件
        if (bean.SelectConditions is not null && bean.SelectConditions.Count > 0)
        {
            builder.Append(
                $" where {string.Join(" and ", bean.SelectConditions.Select(condition => $" ({condition}) "))}");
        }
        // 配置排序规则
        if (bean.OrderByConditions is not null && bean.OrderByConditions.Count > 0)
        {
            builder.Append($" order by {string.Join(", ", bean.OrderByConditions)}");
        }

        return builder.ToString();
    }

    private static string? CheckFollowRule(DbConfigBean bean, Dictionary<string, TableFieldInfo> tableFieldInfos)
    {
        // 检查是否配置了 table
        if (string.IsNullOrWhiteSpace(bean.Table))
        {
            return "Table 不能为空";
        }
        
        // 如果没有配置了 NeedAllFields，则必须配置 Fields
        if (!bean.NeedAllFields && (bean.Fields == null || bean.Fields.Count == 0))
        {
            return "NeedAllFields 为 false 时，必须配置 Fields";
        }
        
        // 检查配置查询的字段是否都存在
        if (!bean.NeedAllFields)
        {
            var notExistsFields = new List<string>();
            foreach (var field in bean.Fields!)
            {
                if (!tableFieldInfos.ContainsKey(field.Name))
                {
                    notExistsFields.Add(field.Name);
                }
            }

            if (notExistsFields.Count > 0)
            {
                return $"{bean.Table} 中不存在的字段: {string.Join(",", notExistsFields)}。请仔细检查配置。";
            }
        }
        


        return null;
    }
}