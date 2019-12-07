using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Naukri.Beatmap
{
    public class Beatmap : BeatmapSimple
    {
        public TimingPoints TimingPoints { get; }

        public Colours Colours { get; }

        public HitObjects HitObjects { get; }

        public Beatmap(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
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
                            break;
                        case "[TimingPoints]":
                            TimingPoints = new TimingPoints(sr);
                            break;
                        case "[Colours]":
                            Colours = new Colours(sr);
                            break;
                        case "[HitObjects]":
                            HitObjects = new HitObjects(sr);
                            break;
                    }
                }
            }
        }
    }
}