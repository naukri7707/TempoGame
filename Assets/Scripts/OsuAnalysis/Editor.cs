using System.IO;

namespace Naukri.OsuAnalysis
{
    public class Editor
    {
        public Editor(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null && line.Length > 0)
            {
                string[] pair = line.Split(':');
                if (pair.Length < 2)
                {
                    break;
                }
                switch (pair[0])
                {
                    case "Bookmarks":
                        Bookmarks = pair[1];
                        break;
                    case "DistanceSpacing":
                        DistanceSpacing = decimal.Parse(pair[1]);
                        break;
                    case "BeatDivisor":
                        BeatDivisor = int.Parse(pair[1]);
                        break;
                    case "GridSize":
                        GridSize = int.Parse(pair[1]);
                        break;
                    case "TimelineZoom":
                        TimelineZoom = decimal.Parse(pair[1]);
                        break;
                }
            }
        }

        /// <summary>
        /// 編輯器書籤的逗號分隔時間列表(未格式化) (毫秒) 
        /// </summary>
        public string Bookmarks { get; set; }

        /// <summary>
        /// 功能"距離捕捉"的倍數
        /// </summary>
        public decimal DistanceSpacing { get; set; }

        /// <summary>
        /// 指定用於放置對象的最佳除法。
        /// </summary>
        public int BeatDivisor { get; set; }

        /// <summary>
        /// 功能"網格捕捉"的網格大小
        /// </summary>
        public int GridSize { get; set; }

        /// <summary>
        /// 指定編輯器的時間軸縮放
        /// </summary>
        public decimal TimelineZoom { get; set; }
    }
}