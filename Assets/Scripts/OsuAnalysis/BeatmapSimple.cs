using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Naukri.OsuAnalysis
{
    public class BeatmapSimple
    {
        public General General { get; protected set; }

        public Editor Editor { get; protected set; }

        public Metadata Metadata { get; protected set; }

        public Difficulty Difficulty { get; protected set; }

        public Events Events { get; protected set; }

        protected BeatmapSimple() { }

        public BeatmapSimple(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                int initCount = 0;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    switch (line)
                    {
                        case "[General]":
                            General = new General(sr);
                            if (++initCount == 5)
                            {
                                return;
                            }
                            break;
                        case "[Editor]":
                            Editor = new Editor(sr);
                            if (++initCount == 5)
                            {
                                return;
                            }
                            break;
                        case "[Metadata]":
                            Metadata = new Metadata(sr);
                            if (++initCount == 5)
                            {
                                return;
                            }
                            break;
                        case "[Difficulty]":
                            Difficulty = new Difficulty(sr);
                            if (++initCount == 5)
                            {
                                return;
                            }
                            break;
                        case "[Events]":
                            Events = new Events(sr);
                            if (++initCount == 5)
                            {
                                return;
                            }
                            break;
                    }
                }
            }
        }
    }
}