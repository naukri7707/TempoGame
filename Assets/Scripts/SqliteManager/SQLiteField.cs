using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Data.Sqlite
{
    /// <summary>
    /// SQLite 欄位
    /// </summary>
    public struct SQLiteField
    {
        public SQLiteField(string name, SQLiteDataType type, params string[] suffix)
        {
            Name = name;
            Type = type;
            Suffix = MergeSuffix(suffix);
        }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型態
        /// </summary>
        public SQLiteDataType Type { get; set; }

        /// <summary>
        /// 後綴
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 合併後綴
        /// </summary>
        /// <param name="src">後綴集合</param>
        /// <returns>合併後的後綴</returns>
        private static string MergeSuffix(string[] src)
        {
            switch (src.Length)
            {
                case 0:
                    return "";
                case 1:
                    return src[0];
                default:
                    var sb = new StringBuilder();
                    foreach (var s in src)
                    {
                        sb.Append(s);
                        sb.Append(' ');
                    }
                    sb.Length--;
                    return sb.ToString();
            }
        }
    }
}
