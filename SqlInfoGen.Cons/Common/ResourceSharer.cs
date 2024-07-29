namespace SqlInfoGen.Cons.Common;

public static class ResourceSharer
{
    /// <summary>
    /// 最多允许同时生成文件数量
    /// <remarks>
    /// <para>
    /// <see cref="Environment.ProcessorCount"/> 是获取 cpu 核心数
    /// </para>
    /// </remarks>
    /// </summary>
    private static readonly SemaphoreSlim FileGenLimiter = new(Environment.ProcessorCount >> 1, Environment.ProcessorCount);

    public static async Task FileHandleWithResourceControlAsync(Func<Task> task)
    {
        try
        {
            // 等待信号量
            await FileGenLimiter.WaitAsync();

            // 数据解析与文件生成
            await task();
        }
        finally
        {
            // 释放信号量
            FileGenLimiter.Release();
        }
    }
}