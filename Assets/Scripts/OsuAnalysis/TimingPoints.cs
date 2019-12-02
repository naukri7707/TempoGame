using System.Collections.Generic;
using System.IO;

namespace Naukri.OsuAnalysis
{
    public class TimingPoints
    {
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

        private readonly TimingPoint[] points;

        public TimingPoints(StreamReader sr)
        {
            var pts = new List<TimingPoint>();
            string line;
            while ((line = sr.ReadLine()) != null && line.Length > 0)
            {
                if (line[0].Equals('/') && line[1].Equals('/'))
                {
                    continue;
                }
                string[] data = line.Split(',');
                if (data.Length < 8)
                {
                    break;
                }
                pts.Add(new TimingPoint
                {
                    Time = int.Parse(data[0]),
                    BeatLength = decimal.Parse(data[1]),
                    Meter = int.Parse(data[2]),
                    SampleSet = int.Parse(data[3]),
                    SampleIndex = int.Parse(data[4]),
                    Volume = int.Parse(data[5]),
                    Uninherited = int.Parse(data[6]),
                    Effects = int.Parse(data[7])
                });
            }
            points = pts.ToArray();
        }

        public TimingPoint this[int index] => points[index];

        public int Length => points.Length;
    }
}