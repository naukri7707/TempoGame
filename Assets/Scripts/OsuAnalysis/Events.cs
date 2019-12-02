using System.IO;
using UnityEngine;

namespace Naukri.OsuAnalysis
{
    public class Events
    {
        public Events(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null && line.Length > 0)
            {
                if (line[0].Equals('/') && line[1].Equals('/'))
                {
                    continue;
                }
                string[] data = line.Split(',');
                if (data.Length < 5)
                {
                    break;
                }
                Video = int.Parse(data[0]);
                StartTime = int.Parse(data[1]);
                Filename = data[2].Replace("\"", "");
                Offset = new Vector2(int.Parse(data[3]), int.Parse(data[4]));
            }
        }
        /// <summary>
        /// 背景類型 0 = 圖 1 = 影片
        /// </summary>
        public int Video { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        public int StartTime { get; set; }

        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// 背景偏移量
        /// </summary>
        public Vector2 Offset { get; set; }
    }
}