using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNAGraphics.KernelBundle
{
    public class ContentHandler
    {
        private ContentManager contentManager;
        private Dictionary<string, string> invokes;
        private Dictionary<string, Object> resources;

        public ContentHandler(ContentManager contentManager)
        {
            this.contentManager = contentManager;

            this.invokes = new Dictionary<string, string>();
            this.resources = new Dictionary<string, Object>();
        }

        public void add(string name, string value)
        {
            this.invokes.Add(name, value);
        }

        public Object get(string name)
        {
            if (this.resources.ContainsKey(name))
                return this.resources[name];
            else
                return null;
        }

        public void load()
        {
            foreach (KeyValuePair<string, string> resource in this.invokes)
            {
                if (this.checkIfResourceExistsAs(resource.Value, "texture"))
                    this.resources.Add(resource.Key, this.contentManager.Load<Texture2D>(resource.Value + ".texture"));
                else if (this.checkIfResourceExistsAs(resource.Value, "font"))
                    this.resources.Add(resource.Key, this.contentManager.Load<SpriteFont>(resource.Value + ".font"));
                else if (this.checkIfResourceExistsAs(resource.Value, "effect"))
                    this.resources.Add(resource.Key, this.contentManager.Load<SoundEffect>(resource.Value + ".effect"));
                else if (this.checkIfResourceExistsAs(resource.Value, "sound"))
                    this.resources.Add(resource.Key, this.contentManager.Load<Song>(resource.Value + ".sound"));
                else
                    throw new Exception("No se ha encontrado el recurso especificado como '" + resource.Value + "'.");
            }
        }

        private Boolean checkIfResourceExistsAs(string name, string extension)
        {
            string[] filePaths = Directory.GetFiles(@".\Content\", "*." + extension + ".xnb");
            foreach (string path in filePaths)
            {
                if (String.Compare(path, @".\Content\" + name + "." + extension + ".xnb", true) == 0)
                    return true;
            }

            return false;
        }
    }
}