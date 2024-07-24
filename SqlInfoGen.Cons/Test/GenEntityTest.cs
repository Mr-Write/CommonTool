namespace SqlInfoGen.Cons.Test;

public class GenEntityTest : DbTest
{
    public void TestTableToEntity()
    {
        Db.DbFirst.StringNullable()
            .CreateClassFile("D:\\SystemDefault\\desk\\MvcLearn\\General.Index\\DO", "General.Index.DO");
    }
}