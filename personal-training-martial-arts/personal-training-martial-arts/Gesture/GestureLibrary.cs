using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using personal_training_martial_arts.Posture;
using Microsoft.Xna.Framework;

namespace personal_training_martial_arts.Gesture
{
    public class GestureLibrary
    {



        public Gesture loadGesture(String nombreGesture) {

            XmlDocument xDoc = new XmlDocument();

            xDoc.Load("./gestures/"+nombreGesture + ".xml");
            XmlNodeList gesto = xDoc.GetElementsByTagName("Gesture");
            XmlAttribute nam = ((XmlElement)gesto[0]).GetAttributeNode("name");
            XmlAttribute des = ((XmlElement)gesto[0]).GetAttributeNode("description");
            XmlAttribute dif = ((XmlElement)gesto[0]).GetAttributeNode("difficulty");

            XmlNodeList listaPostures = ((XmlElement)gesto[0]).GetElementsByTagName("Posture");
            PostureInformation[] posturas = new PostureInformation[listaPostures.Count];
            int i = 0;
            foreach (XmlElement pos in listaPostures) {

                posturas[i] = this.loadPostureNode(pos);
                i++;
                
            }
            return new Gesture(nam.Value,posturas ,des.Value, System.Convert.ToInt16(dif.Value));
        
        }

        private  PostureInformation loadPostureNode(XmlElement pos)
        {

            XmlAttribute nam = pos.GetAttributeNode("name");
            XmlAttribute des = pos.GetAttributeNode("description");
            XmlAttribute dif = pos.GetAttributeNode("difficulty");
            XmlNodeList listaJoints = (pos.GetElementsByTagName("joint"));
            Vector3[] joints = new Vector3[20];
            int index = 0;

            foreach (XmlElement nodo in listaJoints)
            {
                XmlAttribute a = nodo.GetAttributeNode("x");
                XmlAttribute b = nodo.GetAttributeNode("y");
                XmlAttribute c = nodo.GetAttributeNode("z");

                joints[index] = new Vector3(float.Parse(a.Value), float.Parse(b.Value), float.Parse(c.Value));
                index++;
            }

            return  new PostureInformation(nam.Value, des.Value, System.Convert.ToInt16(dif.Value), joints);
           

        }



        public Boolean storeGesture(Gesture gesture)
        {
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);


                // Create the root element
                XmlElement rootNode = xmlDoc.CreateElement("Gesture");
                XmlAttribute _Name = xmlDoc.CreateAttribute("name");
                _Name.Value = gesture.name;
                XmlAttribute _Des = xmlDoc.CreateAttribute("description");
                _Des.Value = gesture.description;
                XmlAttribute _Dif = xmlDoc.CreateAttribute("difficulty");
                _Dif.Value = System.Convert.ToString(gesture.difficulty);

                rootNode.Attributes.Append(_Name);
                rootNode.Attributes.Append(_Des);
                rootNode.Attributes.Append(_Dif);
                xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
                xmlDoc.AppendChild(rootNode);

                foreach (PostureInformation p in gesture.postures) {

                    XmlNode Node = this.createPostureNode(p, xmlDoc);
                    rootNode.AppendChild(Node);
                }
                

                //grabamos
                xmlDoc.Save("./gestures/" + gesture.name + ".xml");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return false;
            }

        }



        private XmlNode createPostureNode(PostureInformation p, XmlDocument xmlDoc) {

   
            XmlNode Node = xmlDoc.CreateNode(XmlNodeType.Element,"Posture",null);
            XmlAttribute _Name = xmlDoc.CreateAttribute("name");
            _Name.Value = p.name;
            XmlAttribute _Des = xmlDoc.CreateAttribute("description");
            _Des.Value = p.description;
            XmlAttribute _Dif = xmlDoc.CreateAttribute("difficulty");
            _Dif.Value = System.Convert.ToString(p.difficulty);
            


            Node.Attributes.Append(_Name);
            Node.Attributes.Append(_Des);
            Node.Attributes.Append(_Dif);
            

           
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
                Node.AppendChild(newSub);

               
            }


            return Node;
        }




    }
}
