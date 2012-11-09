using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using personal_training_martial_arts.Posture;

namespace personal_training_martial_arts.Gesture
{
   

    public class Gesture
    {

        public PostureInformation[] postures { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }
        
        public Gesture(string name,PostureInformation[] postures)
        {
            this.name = name;
            this.postures = postures;
        }
    }
}
