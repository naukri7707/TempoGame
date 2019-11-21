using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Naukri.OsuAnalysis
{
    public class General
    {
        public General(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] pair = line.Split(':');
                if (pair.Length < 2)
                {
                    break;
                }
                switch (pair[0])
                {
                    case "AudioFilename":
                        AudioFilename = pair[1];
                        break;
                    case "AudioLeadIn":
                        AudioLeadIn = int.Parse(pair[1]);
                        break;
                    case "PreviewTime":
                        PreviewTime = int.Parse(pair[1]);
                        break;
                    case "Countdown":
                        Countdown = int.Parse(pair[1]);
                        break;
                    case "SampleSet":
                        SampleSet = pair[1];
                        break;
                    case "StackLeniency":
                        StackLeniency = decimal.Parse(pair[1]);
                        break;
                    case "Mode":
                        Mode = int.Parse(pair[1]);
                        break;
                    case "LetterboxInBreaks":
                        LetterboxInBreaks = int.Parse(pair[1]) == 1;
                        break;
                    case "StoryFireInFront":
                        StoryFireInFront = int.Parse(pair[1]) == 1;
                        break;
                    case "SkinPreference":
                        SkinPreference = pair[1];
                        break;
                    case "EpilepsyWarning":
                        EpilepsyWarning = int.Parse(pair[1]) == 1;
                        break;
                    case "CountdownOffset":
                        CountdownOffset = int.Parse(pair[1]);
                        break;
                    case "WidescreenStoryboard":
                        WidescreenStoryboard = int.Parse(pair[1]) == 1;
                        break;
                    case "SpecialStyle":
                        SpecialStyle = int.Parse(pair[1]) == 1;
                        break;
                    case "UseSkinSprites":
                        UseSkinSprites = int.Parse(pair[1]) == 1;
                        break;
                }
            }
        }
        /// <summary>
        /// 音樂相對於當前資料夾的位置
        /// </summary>
        public string AudioFilename { get; set; }

        /// <summary>
        /// 遊戲開使播放音樂前的延遲時間 (毫秒)
        /// </summary>
        public int AudioLeadIn { get; set; }

        /// <summary>
        /// 歌曲在選擇清單中撥放音樂的時間 (毫秒)
        /// </summary>
        public int PreviewTime { get; set; }

        /// <summary>
        /// specifies the speed of the countdown which occurs before the first hit object appears. (0=No countdown, 1=Normal, 2=Half, 3=Double)
        /// </summary>
        public int Countdown { get; set; }

        /// <summary>
        /// 指定遊戲中的打擊音效
        /// </summary>
        public string SampleSet { get; set; }

        /// <summary>
        /// how often closely placed hit objects will be stacked together.
        /// </summary>
        public decimal StackLeniency { get; set; }

        /// <summary>
        /// 定義遊戲模式 (0=osu!, 1=Taiko, 2=Catch the Beat, 3=osu!mania)
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// 是否在休息期間顯示信箱
        /// </summary>
        public bool LetterboxInBreaks { get; set; }

        /// <summary>
        /// 是否在連擊前顯示情節提要
        /// </summary>
        public bool StoryFireInFront { get; set; }

        /// <summary>
        /// 指定遊戲中使用的首選外觀。
        /// </summary>
        public string SkinPreference { get; set; }

        /// <summary>
        /// 癲癇警告 (在遊戲開始時顯示遊戲畫面是否含有高頻聲光閃爍的場景)
        /// </summary>
        public bool EpilepsyWarning { get; set; }

        /// <summary>
        /// 指定倒數計時開始之前的節拍數
        /// </summary>
        public int CountdownOffset { get; set; }

        /// <summary>
        /// 指定故事板是否應為寬屏。
        /// </summary>
        public bool WidescreenStoryboard { get; set; }

        /// <summary>
        /// 指定是否對osu！mania使用特殊的N+1樣式。
        /// </summary>
        public bool SpecialStyle { get; set; }

        /// <summary>
        /// 指定情節提要是否可以使用用戶的皮膚資源。
        /// </summary>
        public bool UseSkinSprites { get; set; }
    }

    public class Editor
    {
        public Editor(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] pair = line.Split(':');
                if (pair.Length < 2)
                {
                    break;
                }
                switch (pair[0])
                {
                    case "Bookmarks":
                        Bookmarks = int.Parse(pair[1]);
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
        /// 編輯器書籤的逗號分隔時間列表 (毫秒)
        /// </summary>
        public int Bookmarks { get; set; }

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

    public class Metadata
    {
        public Metadata(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
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
        public string Title { get; set; }

        /// <summary>
        /// 標題 (Unicode) 若不支援 Unicode 則使用 ASCII
        /// </summary>
        public string TitleUnicode { get; set; }

        /// <summary>
        /// 歌手名稱 (ASCII)
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// 歌手名稱 (Unicode) 若不支援 Unicode 則使用 ASCII
        /// </summary>
        public string ArtistUnicode { get; set; }

        /// <summary>
        /// Beatmap 的製作玩家名稱
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 歌曲難度
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 歌曲來源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 歌曲標籤集合，用於搜索
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 這個 Beatmap 的 ID
        /// </summary>
        public int BeatmapID { get; set; }

        /// <summary>
        /// 這組 Beatmap 集合的 ID
        /// </summary>
        public int BeatmapSetID { get; set; }
    }

    public class Difficulty
    {
        public Difficulty(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] pair = line.Split(':');
                if (pair.Length < 2)
                {
                    break;
                }
                switch (pair[0])
                {
                    case "HPDrainRate":
                        HPDrainRate = decimal.Parse(pair[1]);
                        break;
                    case "CircleSize":
                        CircleSize = int.Parse(pair[1]);
                        break;
                    case "OverallDifficulty":
                        OverallDifficulty = decimal.Parse(pair[1]);
                        break;
                    case "ApproachRate":
                        ApproachRate = int.Parse(pair[1]);
                        break;
                    case "SliderMultiplier":
                        SliderMultiplier = decimal.Parse(pair[1]);
                        break;
                    case "SliderTickRate":
                        SliderTickRate = decimal.Parse(pair[1]);
                        break;
                }
            }
        }
        /// <summary>
        /// HPDrainRate (HP) specifies how fast the health decreases.
        /// </summary>
        public decimal HPDrainRate { get; set; }

        /// <summary>
        /// CircleSize (CS) defines the size of the hit objects in the osu!standard mode.
        /// </summary>
        public int CircleSize { get; set; }

        /// <summary>
        /// OverallDifficulty (OD) is the harshness of the hit window and the difficulty of spinners.
        /// </summary>
        public decimal OverallDifficulty { get; set; }

        /// <summary>
        /// ApproachRate (AR) defines when hit objects start to fade in relatively to when they should be hit.
        /// </summary>
        public int ApproachRate { get; set; }

        /// <summary>
        /// SliderMultiplier (Decimal) specifies the multiplier of the slider velocity.
        /// </summary>
        public decimal SliderMultiplier { get; set; }

        /// <summary>
        /// SliderTickRate (Decimal) is the number of ticks per beat. The default value is 1 tick per beat.
        /// </summary>
        public decimal SliderTickRate { get; set; }
    }

    public class Events
    {
        public Events(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line[0].Equals('/') && line[1].Equals('/'))
                {
                    continue;
                }
                string[] backdata = line.Split(',');
                if (backdata.Length < 5)
                {
                    break;
                }

                Video = int.Parse(backdata[0]);
                StartTime = int.Parse(backdata[1]);
                Filename = backdata[2];
                Offset = new Vector2(int.Parse(backdata[3]), int.Parse(backdata[4]));
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

    public class TimingPoints
    {
        public TimingPoints(StreamReader sr)
        {
            var pts = new List<TimingPoint>();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line[0].Equals('/') && line[1].Equals('/'))
                {
                    continue;
                }
                string[] backdata = line.Split(',');
                if (backdata.Length < 8)
                {
                    break;
                }
                var pt = new TimingPoint();
                pt.Time = int.Parse(backdata[0]);
                pt.BeatLength = decimal.Parse(backdata[1]);
                pt.Meter = int.Parse(backdata[2]);
                pt.SampleSet = int.Parse(backdata[3]);
                pt.SampleIndex = int.Parse(backdata[4]);
                pt.Volume = int.Parse(backdata[5]);
                pt.Uninherited = int.Parse(backdata[6]);
                pt.Effects = int.Parse(backdata[7]);
                pts.Add(pt);
            }
            mCollection = pts.ToArray();
        }

        public TimingPoint this[int index]
        {
            get
            {
                return mCollection[index];
            }
        }

        private readonly TimingPoint[] mCollection;

        public struct TimingPoint
        {
            public int Time { get; set; }

            public decimal BeatLength { get; set; }

            public int Meter { get; set; }

            public int SampleSet { get; set; }

            public int SampleIndex { get; set; }

            public int Volume { get; set; }

            public int Uninherited { get; set; }

            public int Effects { get; set; }
        }
    }

    public class Colours
    {
        // ???
    }


    public class HitObjects
    {
        public struct HitObject
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int Time { get; set; }

            public int Type { get; set; }

            public int HitSound { get; set; }

            public ObjectParam ObjectParams { get; set; }

            public struct ObjectParam
            {
                public int NormalSet { get; set; }
                public int AdditionSet { get; set; }
                public int Index { get; set; }
                public int Volume { get; set; }
                public string Filename { get; set; }
            }
        }
    }

    public class OsuAnalysis
    {
        public OsuAnalysis(string path)
        {
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    switch (line)
                    {
                        case "[General]":
                            General = new General(sr);
                            break;
                        case "[Editor]":
                            Editor = new Editor(sr);
                            break;
                        case "[Metadata]":
                            Metadata = new Metadata(sr);
                            break;
                        case "[Difficulty]":
                            Difficulty = new Difficulty(sr);
                            break;
                        case "[Events]":
                            Events = new Events(sr);
                            TimingPoints a;
                            break;
                            //case "[Timing Points]":
                            //    TimingPoints = new TimingPoints(sr);
                            //    break;
                            //case "[Colours]":
                            //    Colours = new Colours(sr);
                            //    break;
                            //case "[Hit Objects]":
                            //    HitObjects = new HitObjects(sr);
                            //    break;
                    }
                }
            }
        }
        public readonly General General;

        public readonly Editor Editor;

        public readonly Metadata Metadata;

        public readonly Difficulty Difficulty;

        public readonly Events Events;

        public readonly TimingPoints TimingPoints;

        public readonly Colours Colours;

        public readonly HitObjects HitObjects;
    }
}