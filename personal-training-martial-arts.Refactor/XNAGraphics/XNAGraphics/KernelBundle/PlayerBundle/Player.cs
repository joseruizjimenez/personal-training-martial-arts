using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XNAGraphics.KernelBundle.PlayerBundle
{
    [Serializable()]
    public class Player : ISerializable
    {
        public int id;
        public List<GameScore> gameScoreRecord;

        public Player(int id) 
        {
            this.id = id;
            this.gameScoreRecord = new List<GameScore>();
        }

        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            this.id = (int) info.GetValue("id", typeof(int));
            this.gameScoreRecord = (List<GameScore>) info.GetValue("gameScoreRecord", typeof(List<GameScore>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue
        }

        public void save()
        {
            Stream stream = File.Open(this.id.ToString() + ".player", FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, this);
            stream.Close();
        }

        public void load()
        {
            Player player;
            Stream stream = File.Open(this.id.ToString()+".player", FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            player = (Player)bFormatter.Deserialize(stream);
            stream.Close();
            this.gameScoreRecord = player.gameScoreRecord;
        }
    }
}
