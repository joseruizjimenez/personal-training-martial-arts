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

        public double getTotalScore() {

            double res = 0;
            foreach (var pair in gameScores)
            {
                double valor =  pair.Value;
                res = res + valor;
                
            }
            double score = res / gameScores.Count;
            double rest = 1 - score;

            rest = (rest) * 100;
            int total = (int)rest;
            return total + 50;
        }

        public Dictionary<string, double> convert(Dictionary<PostureInformation, double> gameScoresWithPostures )
        {
            Dictionary<string, double> scores = new Dictionary<string, double>();
            foreach (PostureInformation posture in gameScoresWithPostures.Keys)
            {
                scores.Add(posture.name, this.gameScores[posture]);
            }
            return scores;
        }

       /* byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(gameScores);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(gameScores);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(gameScores);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(gameScores);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(gameScores);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(gameScores);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(gameScores);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(gameScores);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(gameScores);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(gameScores);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return "( " + gameScores.ToString() + " )";
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(gameScores, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(gameScores);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(gameScores);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(gameScores);
        }
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            if (gameScores!= null)
                return true;
            else
                return false;
        }*/
    }
}
