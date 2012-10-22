using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using System.IO;

namespace personal_training_martial_arts.Core
{
    class ContentHandler
    {
        private ContentManager contentManager;
        private Dictionary<string, string> invokes;
        private Dictionary<string, Object> resources;

        public ContentHandler(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            
            this.invokes   = new Dictionary<string, string>();
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
                if (this.checkIfResourceExistsAs(resource.Value, "png"))
                    this.resources.Add(resource.Key, this.contentManager.Load<Texture2D>(resource.Value));
                else if (this.checkIfResourceExistsAs(resource.Value, "spritefont"))
                    this.resources.Add(resource.Key, this.contentManager.Load<SpriteFont>(resource.Value));
                else if (this.checkIfResourceExistsAs(resource.Value, "wav"))
                    this.resources.Add(resource.Key, this.contentManager.Load<SoundEffect>(resource.Value));
                else if (this.checkIfResourceExistsAs(resource.Value, "mp3"))
                    this.resources.Add(resource.Key, this.contentManager.Load<Song>(resource.Value));
                else
                    throw new Exception("No se ha encontrado el recurso especificado como '" + resource.Value + "'.");
            }
        }

        private Boolean checkIfResourceExistsAs(string name, string extension)
        {
            string[] filePaths = Directory.GetFiles(".", "*." + extension);
            foreach (string path in filePaths)
            {
                if (String.Compare(path, @".\" + name + "." + extension, true) == 0)
                    return true;
            }

            return false;
        }
    }
}
