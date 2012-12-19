using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace XNAGraphics.KernelBundle.PlayerBundle
{
    [Serializable()]
    public class PlayerPhoto : ISerializable
    {
        public float[] foto;

        public PlayerPhoto(Texture2D foto) 
        {
            if (foto == null)
                this.foto = new float[921600];
            else
            {
                // 4 por RGB + alpha
                this.foto = new float[foto.Height * foto.Width * 3];
                Color[] colors = new Color[foto.Height * foto.Width];
                foto.GetData(colors);
                int i = 0;
                foreach (Color c in colors)
                {
                    Vector3 v = c.ToVector3();
                    this.foto[i] = v.X;
                    this.foto[i + 1] = v.Y;
                    this.foto[i + 2] = v.Z;
                    i += 3;
                }
            }
        }

        public PlayerPhoto(SerializationInfo info, StreamingContext ctxt)
        {
            this.foto = (float[])info.GetValue("foto", typeof(float[]));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("foto", this.foto);
        }

        public Texture2D getTexture2D(ref Texture2D tex)
        {
            if (tex == null)
                return null;
            Color[] colors = new Color[this.foto.Length / 3];
            for (int i = 0; i < colors.Length; i++ )
            {
                colors[i] = new Color(this.foto[i*3], this.foto[(i*3) + 1], this.foto[(i*3) + 2]);
            }

            tex.SetData(colors);
            return tex;
        }

    }
}
