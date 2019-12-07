using System.IO;

namespace Naukri.Beatmap
{
    public class Difficulty
    {
        public Difficulty(StreamReader sr)
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
                    case "HPDrainRate":
                        HPDrainRate = decimal.Parse(pair[1]);
                        break;
                    case "CircleSize":
                        CircleSize = float.Parse(pair[1]);
                        break;
                    case "OverallDifficulty":
                        OverallDifficulty = decimal.Parse(pair[1]);
                        break;
                    case "ApproachRate":
                        ApproachRate = float.Parse(pair[1]);
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
        public float CircleSize { get; set; }

        /// <summary>
        /// OverallDifficulty (OD) is the harshness of the hit window and the difficulty of spinners.
        /// </summary>
        public decimal OverallDifficulty { get; set; }

        /// <summary>
        /// ApproachRate (AR) defines when hit objects start to fade in relatively to when they should be hit.
        /// </summary>
        public float ApproachRate { get; set; }

        /// <summary>
        /// SliderMultiplier (Decimal) specifies the multiplier of the slider velocity.
        /// </summary>
        public decimal SliderMultiplier { get; set; }

        /// <summary>
        /// SliderTickRate (Decimal) is the number of ticks per beat. The default value is 1 tick per beat.
        /// </summary>
        public decimal SliderTickRate { get; set; }
    }
}