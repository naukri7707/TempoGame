using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Naukri.OsuAnalysis
{
    public class Song
    {
        public Song(string path)
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