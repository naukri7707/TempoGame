using System.Collections.Generic;
using System.IO;

namespace Naukri.Beatmap
{
    public class HitObjects
    {
        public const int MIN_HOLDTIME = 30;

        public struct HitSample
        {
            public int NormalSet { get; set; }
            public int AdditionSet { get; set; }
            public int Index { get; set; }
            public int Volume { get; set; }
            public string Filename { get; set; }
        }

        public struct HitObject
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int Time { get; set; }

            public int Type { get; set; }

            public int HitSound { get; set; }

            public int EndTime { get; set; }

            public HitSample HitSample { get; set; }
        }

        public HitObjects(StreamReader sr)
        {
            var objs = new List<HitObject>();
            string line;
            while ((line = sr.ReadLine()) != null && line.Length > 0)
            {
                if (line[0].Equals('/') && line[1].Equals('/'))
                {
                    continue;
                }
                var data = line.Split(',');
                var sampleData = data[data.Length - 1].Split(':');
                var type = int.Parse(data[3]);
                // Normal Note
                if (type == 1)
                {
                    objs.Add(new HitObject
                    {
                        X = int.Parse(data[0]),
                        Y = int.Parse(data[1]),
                        Time = int.Parse(data[2]),
                        Type = type,
                        HitSound = int.Parse(data[4]),
                        EndTime = int.Parse(data[2]) + MIN_HOLDTIME,
                        HitSample = new HitSample
                        {
                            NormalSet = sampleData.Length > 0 ? int.Parse(sampleData[0]) : 0,
                            AdditionSet = sampleData.Length > 1 ? int.Parse(sampleData[1]) : 0,
                            Index = sampleData.Length > 2 ? int.Parse(sampleData[2]) : 0,
                            Volume = sampleData.Length > 3 ? int.Parse(sampleData[3]) : 0,
                            Filename = sampleData.Length > 4 ? sampleData[4] : ""
                        }
                    });
                }
                // Hold
                else if (type == 128)
                {
                    objs.Add(new HitObject
                    {
                        X = int.Parse(data[0]),
                        Y = int.Parse(data[1]),
                        Time = int.Parse(data[2]),
                        Type = type,
                        HitSound = int.Parse(data[4]),
                        EndTime = int.Parse(sampleData[0]),
                        HitSample = new HitSample
                        {
                            NormalSet = sampleData.Length > 1 ? int.Parse(sampleData[1]) : 0,
                            AdditionSet = sampleData.Length > 2 ? int.Parse(sampleData[2]) : 0,
                            Index = sampleData.Length > 3 ? int.Parse(sampleData[3]) : 0,
                            Volume = sampleData.Length > 4 ? int.Parse(sampleData[4]) : 0,
                            Filename = sampleData.Length > 5 ? sampleData[5] : ""
                        }
                    });
                }
               
            }
            Collection = objs.ToArray();
        }


        public HitObject[] Collection { get; }

        public HitObject this[int index] => Collection[index];


    }
}