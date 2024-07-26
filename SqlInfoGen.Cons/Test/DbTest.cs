using NUnit.Framework;
using SqlInfoGen.Cons.Utils;
using SqlSugar;

namespace SqlInfoGen.Cons.Test;

public abstract class DbTest
{
    protected SqlSugarClient Db;

    /// <summary>
    /// 设置 db
    /// 每一个测试方法执行前都会执行
    /// </summary>
    [SetUp]
    public void Setup()
    {
        Db = new SqlSugarClient(new ConnectionConfig
            {
                DbType = DbType.MySql,
                ConnectionString = ConfigUtils.GetMySqlConnectionString(),
                IsAutoCloseConnection = true
            },
            db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // 获取无参数化 SQL，对性能有影响，特别大的 SQL 参数多的，调试使用
                    Console.WriteLine(UtilMethods.GetSqlString(DbType.MySql, sql, pars));
                };
            });
    }
}