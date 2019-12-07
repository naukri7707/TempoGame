using System.IO;

namespace Naukri.OsuAnalysis
{
    public class General
    {
        public General(StreamReader sr)
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
                    case "AudioFilename":
                        while (pair[1].StartsWith(" "))
                        {
                            pair[1] = pair[1].Remove(0, 1);
                        }
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

        /// <summary>
        /// 取得歌曲資訊
        /// </summary>
        /// <param name="path">完整路徑</param>
        /// <returns>歌曲資訊</returns>
        public static General GetGeneral(string path)
        {
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "[General]")
                    {
                        return new General(sr);
                    }
                }
            }
            return null;
        }
    }
}