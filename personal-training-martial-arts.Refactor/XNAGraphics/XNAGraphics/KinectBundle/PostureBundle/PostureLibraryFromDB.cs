using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAGraphics.KernelBundle;

namespace XNAGraphics.KinectBundle.PostureBundle
{
    public static class PostureLibraryFromDB
    {
        public static PostureInformation[] getPostureList()
        {
            List<PostureInformation> allPostures = new List<PostureInformation>();
            SqliteConnector conn = new SqliteConnector();
            List<string> names = conn.postureGetNames();
            foreach (string n in names)
            {
                    allPostures.Add(loadPosture(n));
            }
            conn.CloseConnection();
            return allPostures.ToArray();
        }

        public static Boolean storePosture(PostureInformation p)
        {
            SqliteConnector conn = new SqliteConnector();
            if (conn.postureGetId(p.name) != "")
            {
                conn.CloseConnection();
                return false;
            }
            conn.insert("INSERT INTO POSTURE (NAME,DESCRIPTION,DIFFICULTY) VALUES ('" + p.name + "','" + p.description + "'," + System.Convert.ToString(p.difficulty) + ")");
            string postid = conn.postureGetId(p.name);
            int index = 0;
            foreach (Vector3 v in p.joints)
            {
                conn.insert("INSERT INTO JOINT (ID_POSTURE,JOINT_ORDER,X,Y,Z) VALUES (" + postid + "," + index + "," + v.X.ToString().Replace(",", ".") + "," + v.Y.ToString().Replace(",", ".") + "," + v.Z.ToString().Replace(",", ".") + ")");
                index++;
            }
            conn.CloseConnection();
            return true;
        }

        public static PostureInformation loadPosture(String name)
        {
            Vector3[] joints = new Vector3[20];
            int index = 0;
            SqliteConnector conn = new SqliteConnector();
            string postid = conn.postureGetId(name);
            if (postid == "")
            {
                conn.CloseConnection();
                return null;
            }
            Tuple<string,string,string> tPost = conn.postureGetData(name);
            if (tPost == null)
            {
                conn.CloseConnection();
                return null;
            }
            List<Tuple<string, string, string, string>> tJoint= conn.postureGetJoints(postid);
            if (tJoint.Count!=20)
            {
                conn.CloseConnection();
                return null;
            }

            foreach (Tuple<string, string, string, string> joint in tJoint)
            {
                joints[System.Convert.ToInt16(joint.Item1)] = new Vector3(float.Parse(joint.Item2), float.Parse(joint.Item3), float.Parse(joint.Item4));
                index++;
            }

            PostureInformation postureI = new PostureInformation(tPost.Item1, tPost.Item2, System.Convert.ToInt16(tPost.Item3), joints);
            conn.CloseConnection();
            return postureI;
        }
    }
}
