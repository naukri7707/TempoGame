using System.Collections.Generic;
using System.IO;

namespace Naukri.Beatmap
{
    public class Metadata
    {
        public Metadata(StreamReader sr)
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
                    case "Title":
                        Title = pair[1];
                        break;
                    case "TitleUnicode":
                        TitleUnicode = pair[1];
                        break;
                    case "Artist":
                        Artist = pair[1];
                        break;
                    case "ArtistUnicode":
                        ArtistUnicode = pair[1];
                        break;
                    case "Creator":
                        Creator = pair[1];
                        break;
                    case "Version":
                        Version = pair[1];
                        break;
                    case "Source":
                        Source = pair[1];
                        break;
                    case "Tags":
                        Tags = new List<string>(pair[1].Split(' '));
                        for (int i = 0; i < Tags.Count; i++)
                        {
                            Tags[i] = Tags[i].Replace('_', ' ');
                        }
                        break;
                    case "BeatmapID":
                        BeatmapID = int.Parse(pair[1]);
                        break;
                    case "BeatmapSetID":
                        BeatmapSetID = int.Parse(pair[1]);
                        break;
                }
            }
        }

        /// <summary>
        /// 標題 (ASCII)
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 標題 (Unicode) 
        /// </summary>
        public string TitleUnicode { get; }

        /// <summary>
        /// 正確的標題 若不支援 Unicode 則使用 ASCII
        /// </summary>
        public string TrueTitle => TitleUnicode == "" ? Title : TitleUnicode;

        /// <summary>
        /// 音樂家名稱 (ASCII)
        /// </summary>
        public string Artist { get; }

        /// <summary>
        /// 音樂家名稱 (Unicode) 若不支援 Unicode 則使用 ASCII
        /// </summary>
        public string ArtistUnicode { get; }

        /// <summary>
        /// 正確的音樂家 若不支援 Unicode 則使用 ASCII
        /// </summary>
        public string TrueArtist => ArtistUnicode == "" ? Artist : ArtistUnicode;

        /// <summary>
        /// Beatmap 的製作玩家名稱
        /// </summary>
        public string Creator { get; }

        /// <summary>
        /// 歌曲難度
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// 歌曲來源
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// 歌曲標籤集合，用於搜索
        /// </summary>
        public List<string> Tags { get; }

        /// <summary>
        /// 這個 Beatmap 的 ID
        /// </summary>
        public int BeatmapID { get; }

        /// <summary>
        /// 這組 Beatmap 集合的 ID
        /// </summary>
        public int BeatmapSetID { get; }


        /// <summary>
        /// 取得歌曲資訊
        /// </summary>
        /// <param name="path">完整路徑</param>
        /// <returns>歌曲資訊</returns>
        public static Metadata GetMetadata(string path)
        {
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "[Metadata]")
                    {
                        return new Metadata(sr);
                    }
                }
            }
            return null;
        }
    }
}