using System.Data;
using System.Text;
using System.Text.Json;
using NUnit.Framework;
using SqlInfoGen.Cons.Bean;
using SqlInfoGen.Cons.Bean.Config;
using SqlInfoGen.Cons.Common;
using SqlInfoGen.Cons.Enums;
using SqlInfoGen.Cons.Helpers;
using SqlInfoGen.Cons.Utils;
using SqlInfoGen.Cons.Utils.ViewGen;

namespace SqlInfoGen.Cons.Test;

public class GenDataFileTest : DbTest
{
    [Test]
    public async Task TestGenDataFile()
    {
        var dbBeans = ConfigUtils.GetDbConfigBeanList();

        // 一个文件对应一份数据库，包含了多个表的数据与结构
        await DbFileGenHelper.GenDbTableFileBatchAsync(dbBeans);
    }

}