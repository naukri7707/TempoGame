
static class GameArgs
{
    /// <summary>
    /// 主執行緒 ID
    /// </summary>
    private static int mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

    /// <summary>
    /// 判斷是否為主執行緒
    /// </summary>
    public static bool IsMainThread => System.Threading.Thread.CurrentThread.ManagedThreadId == mainThreadId;

}