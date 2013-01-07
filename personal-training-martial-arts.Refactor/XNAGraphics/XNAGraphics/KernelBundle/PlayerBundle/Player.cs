using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAGraphics.KernelBundle.PlayerBundle
{
    [Serializable()]
    public class Player : ISerializable
    {
        public int id;
        //public List<GameScore2> gameScoreRecord;
        public Boolean active;
        public Texture2D foto;

        public List<float> foto_serializable;
        public List<double> lista_Resultados;
        public List<DateTime> lista_Fechas;

        public Player(int id, Texture2D foto)
        {
            this.id = id;
            //this.gameScoreRecord = new List<GameScore2>();
            //this.gameScoreRecord.Add(new GameScore2());
            this.active = false;
            this.foto_serializable = new List<float>();
            this.foto = foto;
            this.lista_Resultados = new List<double>();
            this.lista_Fechas = new List<DateTime>();
        }

        public Player(SerializationInfo info, StreamingContext ctxt)
        {
            this.id = (int)info.GetValue("id", typeof(int));
            //this.gameScoreRecord = (List<GameScore2>)info.GetValue("gameScoreRecord", typeof(List<GameScore>));
            this.active = (Boolean)info.GetBoolean("active");
            this.foto_serializable = (List<float>)info.GetValue("foto_serializable", typeof(List<float>));
            this.lista_Resultados = (List<double>)info.GetValue("lista_Resultados", typeof(List<double>));
            this.lista_Fechas = (List<DateTime>)info.GetValue("lista_Fechas", typeof(List<DateTime>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("id", this.id);
            //info.AddValue("gameScoreRecord", this.gameScoreRecord);
            info.AddValue("active", this.active);
            info.AddValue("foto_serializable", getSerializedPhoto());
            info.AddValue("lista_Resultados", this.lista_Resultados);
            info.AddValue("lista_Fechas", this.lista_Fechas);
        }

        public void save()
        {
            Stream stream = File.Open(this.id.ToString() + ".player", FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, this);
            stream.Close();
        }

        public void load(Texture2D texture)
        {
            Player player;
            Stream stream=null;
            BinaryFormatter bFormatter;
            try
            {
                stream = File.Open(this.id.ToString() + ".player", FileMode.Open);
                bFormatter = new BinaryFormatter();
                player = (Player)bFormatter.Deserialize(stream);
                stream.Close();
                this.id = player.id;
                this.active = player.active;
                this.lista_Resultados = player.lista_Resultados;
                this.lista_Fechas = player.lista_Fechas;
               // this.gameScoreRecord = player.gameScoreRecord;
                this.foto = texture;
                if (player.foto_serializable != null && player.foto_serializable.Count != 0)
                    this.foto.SetData(setSerializedPhoto(player.foto_serializable));
            }
            catch (System.Exception e)
            {
                e.GetBaseException();

            }
            finally {
                stream.Close();
            
            }
            
        }

        public static Player loadPlayer(int id, Texture2D foto)
        {
            Player player;
            Stream stream ;
            BinaryFormatter bFormatter;
            try
            {
                stream = File.Open(id.ToString() + ".player", FileMode.Open);
                bFormatter = new BinaryFormatter();
                player = (Player)bFormatter.Deserialize(stream);
                stream.Close();
            }
            // si el jugador no existe crea su ficha de nuevo jugador
            catch (System.IO.FileNotFoundException e)
            {
                e.GetType();
                player = new Player(id, foto);
                player.save();
               

            }
            finally
            {
               

            }

            return player;
        }

        public string getImageName()
        {
            return "player_" + this.id.ToString();
        }

        private List<float> getSerializedPhoto()
        {
            List<float> f = new List<float>();
            if (this.foto != null)
            {
                // 4 por RGB + alpha
                //f = new float[foto.Height * foto.Width * 3];
                Color[] colors = new Color[foto.Height * foto.Width];
                foto.GetData(colors);
                int i = 0;
                foreach (Color c in colors)
                {
                    Vector3 v = c.ToVector3();
                    f.Add(v.X);
                    f.Add(v.Y);
                    f.Add(v.Z);
                    i += 3;
                }
            }
            return f;
        }

        private Color[] setSerializedPhoto(List<float> foto_serializable)
        {
            if (this.foto == null)
                return new Color[640 * 480];
            Color[] colors = new Color[foto_serializable.Count / 3];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(foto_serializable[i * 3], foto_serializable[(i * 3) + 1], foto_serializable[(i * 3) + 2]);
            }

            return colors;
        }


        public string getHistoric() {
            string text="";
            int counter = 0;
            for (int i = (lista_Resultados.Count -1);(i >= 0) && (counter < 3)  ;i-- )
            {
                text = text + " \n ";    
                text = text + " " + lista_Resultados[i].ToString()+"%             " + lista_Fechas[i].ToString();
                counter++;
            }

            return text;


        
        }


    }
}
