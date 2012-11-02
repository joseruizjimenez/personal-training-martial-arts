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
    public static class PostureLibrary
    {
        public static PostureInformation[] getPostureList()
        {
            String[] nombresficheros = System.IO.Directory.GetFiles("./postures/");
            int a = 0;
            foreach (String n in nombresficheros)
            {
                if (n.Contains(".xml")) a++;
            }

            PostureInformation[] allPostures = new PostureInformation[a];
            a = 0;
            foreach (String n in nombresficheros)
            {
                if (n.Contains(".xml"))
                {
                    allPostures[a] = loadPosture(n.Replace(".xml", ""));
                    a++;
                }
            }

            return allPostures;
        }


        public static Boolean storePosture(PostureInformation p)
        {
            try
            {
                //XmlDocument originalXml = new XmlDocument();
                //originalXml.CreateXmlDeclaration("1.0", "utf-8", null);

                XmlDocument xmlDoc = new XmlDocument();
                // Write down the XML declaration
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);


                // Create the root element
                XmlElement rootNode = xmlDoc.CreateElement("Posture");
                XmlAttribute _Name = xmlDoc.CreateAttribute("name");
                _Name.Value = p.name;
                XmlAttribute _Des = xmlDoc.CreateAttribute("description");
                _Des.Value = p.description;
                XmlAttribute _Dif = xmlDoc.CreateAttribute("difficulty");
                _Dif.Value = System.Convert.ToString(p.difficulty);

                rootNode.Attributes.Append(_Name);
                rootNode.Attributes.Append(_Des);
                rootNode.Attributes.Append(_Dif);
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                xmlDoc.AppendChild(rootNode);

                int index = 0;
                foreach (Vector3 v in p.joints)
                {

                    XmlNode newSub = xmlDoc.CreateNode(XmlNodeType.Element, "joint", null);
                    XmlAttribute _X = xmlDoc.CreateAttribute("x");
                    _X.Value = System.Convert.ToString(v.X);
                    XmlAttribute _Y = xmlDoc.CreateAttribute("y");
                    _Y.Value = System.Convert.ToString(v.Y);
                    XmlAttribute _Z = xmlDoc.CreateAttribute("z");
                    _Z.Value = System.Convert.ToString(v.Z);

                    //los agregamos
                    newSub.Attributes.Append(_X);
                    newSub.Attributes.Append(_Y);
                    newSub.Attributes.Append(_Z);
                    rootNode.AppendChild(newSub);

                    index++;
                }

                //grabamos
                xmlDoc.Save("./postures/" + p.name + ".xml");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            }
        }

        public static PostureInformation loadPosture(String name)
        {
            Vector3[] joints = new Vector3[20];
            int index = 0;
            XmlDocument xDoc = new XmlDocument();

            //xDoc.Load("../../../../personas1.xml");
            xDoc.Load(name + ".xml");
            XmlNodeList postura = xDoc.GetElementsByTagName("Posture");

            XmlAttribute nam = ((XmlElement)postura[0]).GetAttributeNode("name");
            XmlAttribute des = ((XmlElement)postura[0]).GetAttributeNode("description");
            XmlAttribute dif = ((XmlElement)postura[0]).GetAttributeNode("difficulty");

            XmlNodeList listaJoints = ((XmlElement)postura[0]).GetElementsByTagName("joint");

            foreach (XmlElement nodo in listaJoints)
            {
                XmlAttribute a = nodo.GetAttributeNode("x");
                XmlAttribute b = nodo.GetAttributeNode("y");
                XmlAttribute c = nodo.GetAttributeNode("z");

                joints[index] = new Vector3(float.Parse(a.Value), float.Parse(b.Value), float.Parse(c.Value));
                index++;
            }

            PostureInformation postureI = new PostureInformation(nam.Value, des.Value, System.Convert.ToInt16(dif.Value), joints);
            return postureI;

        }
    }
}