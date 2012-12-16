using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGraphics.KinectBundle.PostureBundle;
using System.Runtime.Serialization;

namespace XNAGraphics.KernelBundle.PlayerBundle
{
    [Serializable()]
    public class GameScore : ISerializable
    {
        public Dictionary<PostureInformation, double> gameScores;
        public DateTime date;

        public Dictionary<string, double> GameScores
        {
            get { return getSerializedGameScore(); }
            set { this.gameScores = setSerializedGameScore(value); }
        }

        public GameScore()
        {
            gameScores = new Dictionary<PostureInformation, double>();
            date = System.DateTime.Now;
        }

        public GameScore(SerializationInfo info, StreamingContext ctxt)
        {
            this.date = (DateTime)info.GetValue("date", typeof(DateTime));
            this.GameScores = (Dictionary<string, double>) info.GetValue("GameScores", typeof(Dictionary<string, double>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("GameScores", typeof(Dictionary<string, double>));
            info.AddValue("date", typeof(DateTime));
        }

        public Dictionary<string, double> getSerializedGameScore()
        {
            Dictionary<string, double> scores = new Dictionary<string, double>();
            foreach (PostureInformation posture in this.gameScores.Keys)
            {
                scores.Add(posture.name, this.gameScores[posture]);
            }
            return scores;
        }

        public Dictionary<PostureInformation, double> setSerializedGameScore(Dictionary<string, double> s)
        {
            Dictionary<PostureInformation, double> g = new Dictionary<PostureInformation, double>();
            foreach (string name in s.Keys)
            {
                g.Add(PostureLibraryFromDB.loadPosture(name), s[name]);
            }
            return g;
        }
    }
}
