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
                var data = line.Split(',');
                if (data.Length >= 3)
                {
                    if (int.TryParse(data[0], out int video))
                    {
                        Video = video;
                        if (video == 0)
                        {
                            StartTime = int.Parse(data[1]);
                            Filename = data[2].Replace("\"", "");
                            if (data.Length == 3)
                            {
                                Offset = new Vector2(0, 0);
                            }
                            else
                            {
                                Offset = new Vector2(int.Parse(data[3]), int.Parse(data[4]));
                            }
                        }
                    }
                }
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

        /// <summary>
        /// 取得歌曲事件集
        /// </summary>
        /// <param name="path">完整路徑</param>
        /// <returns>歌曲事件集</returns>
        public static Events GetEvents(string path)
        {
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "[Events]")
                    {
                        return new Events(sr);
                    }
                }
            }
            return null;
        }
    }
}