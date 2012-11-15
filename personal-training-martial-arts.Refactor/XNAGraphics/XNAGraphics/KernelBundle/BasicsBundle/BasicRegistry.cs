using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGraphics.KernelBundle.BasicsBundle
{
    class BasicRegistry
    {
        public List<BasicCollection> components;

        public BasicRegistry()
        {
            this.components = new List<BasicCollection>();
        }

        /// <summary>
        /// Adds new bundle to the components.
        /// </summary>
        /// <param name="layer">A bundle to add.</param>
        public void add(BasicCollection collection)
        {
            this.components.Add(collection);
        }

        public void remove(BasicCollection collection)
        {
            this.components.Remove(collection);
        }

        public void clear()
        {
            this.components.Clear();
        }

        /// <summary>
        /// Sort bundles in components by bundle priority.
        /// </summary>
        public void sortByPriority()
        {
            this.components.Sort(/**/ (x, y) => ( x.priority.CompareTo(y.priority) * (-1) ) /**/);
        }
    }
}