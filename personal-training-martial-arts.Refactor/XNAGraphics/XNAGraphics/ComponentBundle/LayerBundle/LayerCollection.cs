using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNAGraphics.ComponentBundle.LayerBundle
{
    class LayerCollection : XNAGraphics.KernelBundle.BasicsBundle.BasicCollection
    {
        public LayerCollection()
            : base() { }

        public LayerCollection(int priority)
            : base(priority) { }

        public LayerCollection(params Layer[] layers)
            : base(layers) { }

        public LayerCollection(int priority, params Layer[] layers)
            : base(priority, layers) { }

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