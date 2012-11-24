using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGraphics.KernelBundle.BasicsBundle
{
    class BasicCollection
    {
        public string identifier;
        public List<Object> components;
        public int priority;

        public BasicCollection(string identifier)
            : this(identifier, 0) { }

        public BasicCollection(string identifier, params Object[] components)
            : this(identifier, 0, components) { }

        public BasicCollection(string identifier, int priority, Object[] components)
        {
            this.identifier = identifier;
            this.components = new List<Object>();
            this.priority = priority;

            foreach (Object component in components)
            {
                this.add(component);
            }
        }

        /// <summary>
        /// Adds new object to the collection.
        /// </summary>
        /// <param name="layer">An object to add.</param>
        public void add(Object component)
        {
            this.components.Add(component);
        }

        public void remove(Object component)
        {
            this.components.Remove(component);
        }

        public Boolean hasNext()
        {
            //this.components.Add("shit");
            //throw new Exception(">>> "+ this.components.Count);
            if (this.components.Count > 1)
                return true;
            return false;
        }

        public Object first()
        {
            //throw new Exception("ñaau");
            return this.components[1];
        }
    }
}
