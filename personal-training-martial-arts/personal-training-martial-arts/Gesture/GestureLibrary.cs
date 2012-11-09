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
                    Console.WriteLine("putaaaaaaaaaaaaaaaaa4");
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
