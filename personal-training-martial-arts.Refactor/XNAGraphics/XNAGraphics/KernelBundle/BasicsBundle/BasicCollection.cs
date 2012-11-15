using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGraphics.KernelBundle.BasicsBundle
{
    class BasicCollection
    {
        public List<Object> components;
        public int priority;

        public BasicCollection()
            : this(0) { }

        public BasicCollection(params Object[] components)
            : this(0, components) { }

        public BasicCollection(int priority, Object[] components)
        {
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
    }
}
