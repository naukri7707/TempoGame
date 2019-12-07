namespace Mono.Data.Sqlite
{
    /// <summary>
    /// SQLite 資料對
    /// </summary>
    public struct SQLiteDataPair
    {
        /// <summary>
        /// 產生一個帶有 field 和 value 的 SQLiteDataPair
        /// </summary>
        /// <param name="field">欄位</param>
        /// <param name="value">資料</param>
        public SQLiteDataPair(string field, object value)
        {
            Field = field;
            Value = value;
        }

        /// <summary>
        /// 欄位
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 數值
        /// </summary>
        public object Value { get; set; }
    }
}
