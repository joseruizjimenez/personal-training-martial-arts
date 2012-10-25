<<<<<<< HEAD
﻿using System;
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
    static class PostureLibrary
    {
        public static PostureInformation[] getPostureList()
        {





        }

        public static Boolean storePosture(PostureInformation p) {

            // PROBLEMA DEL NOMBRE DEL FICHERO. ME LO PASAN O DEBERIA LA CLASE TENER UN NOMBRE??
            
            try
            {
                XmlDocument originalXml = new XmlDocument();
                originalXml.CreateXmlDeclaration("1.0", "utf-8", null);
                originalXml.Load(p.name+".xml"); 
                XmlNode Elem_joints = originalXml.SelectSingleNode("Posture");
                // atributos de la postura
                XmlAttribute _Name = originalXml.CreateAttribute("name");
                _Name.Value =  p.name;
                XmlAttribute _Des = originalXml.CreateAttribute("description");
                _Des.Value = p.description;
                XmlAttribute _Dif = originalXml.CreateAttribute("difficulty");
                _Dif.Value = System.Convert.ToString(p.description);

                Elem_joints.Attributes.Append(_Name);
                Elem_joints.Attributes.Append(_Des);
                Elem_joints.Attributes.Append(_Dif);

                int index = 0;
                foreach (Vector3 v in p.joints)
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
                originalXml.Save(p.name+".xml");
                return true;
                
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            } 
        
        }


        public static PostureInformation loadPosture(String name) { 
        
                Vector3[] joints = new Vector3[20];
                int index = 0;
                XmlDocument xDoc = new XmlDocument();

               

                //xDoc.Load("../../../../personas1.xml"); 
                xDoc.Load(name+".xml");
                XmlNodeList postura = xDoc.GetElementsByTagName("Posture");

                    XmlAttribute nam= ((XmlElement)postura[0]).GetAttributeNode("name");
                    XmlAttribute des= ((XmlElement)postura[0]).GetAttributeNode("description");
                    XmlAttribute dif= ((XmlElement)postura[0]).GetAttributeNode("difficulty");

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

             
                PostureInformation postureI= new PostureInformation(nam.Value,des.Value,System.Convert.ToInt16(dif.Value) ,joints);
                return postureI;

                

                }
        
        }

        

       
    
}
=======
﻿using System;
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
    static class PostureLibrary
    {
        public static PostureInformation[] getPostureList()
        {
            String [] nombresficheros = System.IO.Directory.GetFiles(".");
            int a=0;
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
                    allPostures[a] = loadPosture(n.Replace(".xml",""));
                    a++;
                } 
            }

            return allPostures;
        }


        public static Boolean storePosture(PostureInformation p) 
        {
            // PROBLEMA DEL NOMBRE DEL FICHERO. ME LO PASAN O DEBERIA LA CLASE TENER UN NOMBRE??
            
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


                //originalXml.Load(p.name+".xml"); 
                //XmlNode Elem_joints = originalXml.SelectSingleNode("Posture");
                // atributos de la postura
                

                

                int index = 0;
                foreach (Vector3 v in p.joints)
                {

                    XmlNode newSub = xmlDoc.CreateNode(XmlNodeType.Element, "joint", null);
                    XmlAttribute _X = xmlDoc.CreateAttribute("x");
                    _X.Value =  System.Convert.ToString(v.X);
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
                xmlDoc.Save(p.name + ".xml");
               
                return true;       
            }
            catch(Exception e)
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
            xDoc.Load(name+".xml");
            XmlNodeList postura = xDoc.GetElementsByTagName("Posture");

            XmlAttribute nam= ((XmlElement)postura[0]).GetAttributeNode("name");
            XmlAttribute des= ((XmlElement)postura[0]).GetAttributeNode("description");
            XmlAttribute dif= ((XmlElement)postura[0]).GetAttributeNode("difficulty");

            XmlNodeList listaJoints =  ((XmlElement)postura[0]).GetElementsByTagName("joint"); 

            foreach (XmlElement nodo in listaJoints)
            {
                XmlAttribute a= nodo.GetAttributeNode("x");
                XmlAttribute b= nodo.GetAttributeNode("y");
                XmlAttribute c= nodo.GetAttributeNode("z");
                
                joints[index] = new Vector3(System.Convert.ToInt64(a.Value),System.Convert.ToInt64(b.Value), 
                System.Convert.ToInt64(c.Value));    index++;
            } 

            PostureInformation postureI= new PostureInformation(nam.Value,des.Value,System.Convert.ToInt16(dif.Value),joints);
            return postureI;

        }
    }       
}
>>>>>>> ae8ab784c7ff7422ed0181609c2acba1bd2e2fc5
