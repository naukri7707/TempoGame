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
                var data = line.Split(',', ':');
                if (data.Length == 10)
                {
                    objs.Add(new HitObject
                    {
                        X = int.Parse(data[0]),
                        Y = int.Parse(data[1]),
                        Time = int.Parse(data[2]),
                        Type = int.Parse(data[3]),
                        HitSound = int.Parse(data[4]),
                        EndTime = int.Parse(data[2]) + MIN_HOLDTIME,
                        HitSample = new HitSample
                        {
                            NormalSet = int.Parse(data[5]),
                            AdditionSet = int.Parse(data[6]),
                            Index = int.Parse(data[7]),
                            Volume = int.Parse(data[8]),
                            Filename = data[9]
                        }
                    });
                }
                else if (data.Length == 11)
                {
                    objs.Add(new HitObject
                    {
                        X = int.Parse(data[0]),
                        Y = int.Parse(data[1]),
                        Time = int.Parse(data[2]),
                        Type = int.Parse(data[3]),
                        HitSound = int.Parse(data[4]),
                        EndTime = int.Parse(data[5]),
                        HitSample = new HitSample
                        {
                            NormalSet = int.Parse(data[6]),
                            AdditionSet = int.Parse(data[7]),
                            Index = int.Parse(data[8]),
                            Volume = int.Parse(data[9]),
                            Filename = data[10]
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