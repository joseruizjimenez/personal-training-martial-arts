using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGraphics.KinectBundle.PostureBundle;
using System.Runtime.Serialization;

namespace XNAGraphics.KernelBundle.PlayerBundle
{
    public class GameScore
    {
        public Dictionary<PostureInformation, double> gameScores;
        public DateTime date;

        public GameScore()
        {
            gameScores = new Dictionary<PostureInformation, double>();
            date = System.DateTime.Now;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("gameScores", typeof(Dictionary<string, double>));
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
    }
}
