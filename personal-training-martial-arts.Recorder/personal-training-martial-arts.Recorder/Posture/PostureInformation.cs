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

namespace personal_training_martial_arts.Posture
{
    class PostureInformation : Posture, IEquatable<PostureInformation>
    {
        public string name { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }

        public PostureInformation(string name, string description, int difficulty, Skeleton skeleton)
            : this(name, description, difficulty, Posture.castSkeletonToJoints(skeleton)){}

        public PostureInformation(string name, string description, int difficulty, Vector3[] joints)
        {
            this.name = name;
            this.description = description;
            this.difficulty = difficulty;
            this.joints = joints;
        }


        public override String GetHashCode()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PostureInformation);
        }

        public bool Equals(PostureInformation obj)
        {
            return obj != null && obj.name == this.name;
        }
    }
}
