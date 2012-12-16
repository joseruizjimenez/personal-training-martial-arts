using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGraphics.ComponentBundle.LayerBundle
{
    public class LayerCollection : XNAGraphics.KernelBundle.BasicsBundle.BasicCollection
    {
        public LayerCollection(string identifier)
            : base(identifier) { }

        public LayerCollection(string identifier, int priority)
            : base(identifier, priority) { }

        public LayerCollection(string identifier, params Layer[] layers)
            : base(identifier, layers) { }

        public LayerCollection(string identifier, int priority, params Layer[] layers)
            : base(identifier, priority, layers) { }

        public Layer get(string identifier)
        {
            foreach (Layer l in this.components)
            {
                if (l.identifier.ToLowerInvariant() == identifier.ToLowerInvariant())
                    return l;
            }
            
            throw new Exception("Layer \"" + identifier + "\" do not exist!");
        }

        /// <summary>
        /// Sort layers in collection by layer priority.
        /// </summary>
        public void sortByPriority()
        {
            Comparison<Object> comparison = new Comparison<Object>(comparePriority);
            this.components.Sort(comparison);
        }

        public int comparePriority(Object x, Object y)
        {
            // INFO: [LayerCollection] Compare priority method.
            Layer layer_x = (Layer) x;
            Layer layer_y = (Layer) y;

            return layer_x.priority.CompareTo(layer_y.priority) * (-1);
        }
    }
}