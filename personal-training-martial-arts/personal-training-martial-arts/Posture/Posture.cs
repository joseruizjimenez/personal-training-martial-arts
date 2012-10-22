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
using System.Xml;

namespace personal_training_martial_arts.Posture
{
    class Posture
    {
        protected Vector3[] joints {get; set;}

        public Posture()
        {
        }

        public Posture(Skeleton skeleton)
            : this(Posture.castSkeletonToJoints(skeleton))
        {
        }

        public Posture(String name)
            : this(Posture.loadFromXML(name))
        {
        }

        public Posture(Vector3[] joints)
        {
            this.joints = joints;
        }

        public static Vector3[] castSkeletonToJoints(Skeleton s)
        {
            Vector3[] joints = new Vector3[20];
            int index = 0;

            foreach (Joint j in s.Joints)
            {
                joints[index] = new Vector3(j.Position.X, j.Position.Y, j.Position.Z);
                index++;
            }

            return joints;
        }

        public Boolean writeToXML(String name) {

            // PROBLEMA DEL NOMBRE DEL FICHERO. ME LO PASAN O DEBERIA LA CLASE TENER UN NOMBRE??
            
            try
            {
                XmlDocument originalXml = new XmlDocument();
                originalXml.CreateXmlDeclaration("1.0", "utf-8", null);
                originalXml.Load(name+".xml"); 
                XmlNode Elem_joints = originalXml.SelectSingleNode("Posture");

                int index = 0;
                foreach (Vector3 v in joints)
                {
                  
                    XmlNode newSub = originalXml.CreateNode(XmlNodeType.Element, "joint", null);
                    XmlAttribute _X = originalXml.CreateAttribute("x");
                    _X.Value =  System.Convert.ToString(v.X);
                    XmlAttribute _Y = originalXml.CreateAttribute("y");
                    _Y.Value = System.Convert.ToString(v.Y);
                    XmlAttribute _Z = originalXml.CreateAttribute("z");
                    _Z.Value = System.Convert.ToString(v.Z);

                    //los agregamos 
                    newSub.Attributes.Append(_X);
                    newSub.Attributes.Append(_Y);
                    newSub.Attributes.Append(_Z);
                    Elem_joints.AppendChild(newSub);
                
                    index++;
                }
                
                //grabamos 
                originalXml.Save(name+".xml");
                return true;
                
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            } 
        
        }


        public static Vector3[] loadFromXML(String name) { 
        
                Vector3[] joints = new Vector3[20];
                int index = 0;
                XmlDocument xDoc = new XmlDocument();

               

                //xDoc.Load("../../../../personas1.xml"); 
                xDoc.Load(name+".xml");
                XmlNodeList postura = xDoc.GetElementsByTagName("Posture");

                XmlNodeList listaJoints =  ((XmlElement)postura[0]).GetElementsByTagName("joint"); 

                foreach (XmlElement nodo in listaJoints)
                {

            
                    XmlAttribute a= nodo.GetAttributeNode("x");
                    XmlAttribute b= nodo.GetAttributeNode("y");
                    XmlAttribute c= nodo.GetAttributeNode("z");
                
                    joints[index] = new Vector3(System.Convert.ToInt64(a.Value),System.Convert.ToInt64(b.Value), 
                                    System.Convert.ToInt64(c.Value));
                    index++;
                

                } 

             

          

                return joints;

                }
        
        }



        public Boolean compareTo(Posture p, float averageTolerance, float puntualTolerance)
        {
            /**
             * @TODO: Normalizar primero y despues comparamos
             */

            return false;
        }
    }
}
