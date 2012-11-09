using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using personal_training_martial_arts.Posture;
using personal_training_martial_arts.Gesture;


namespace personal_training_martial_arts.Tests
{
    /*CLASE DE TEST EN LA QUE VOY A TRABAJAR SIGUIENDO TDD*/
    
    [TestFixture]
    public class TestGestureXML
    {
        /*Metodo que va a porbar si se escribe un xml de gestos, que es una coleccion de posturas*/
        [Test]
        public void TestWrittingGestureinXML()
        {
            GestureLibrary gesLib = new GestureLibrary();
            PostureInformation[] PosturesOfGesture = new PostureInformation[2];
            PosturesOfGesture[0] = PostureLibrary.loadPosture("./postures/T_JOSE");
            PosturesOfGesture[1] = PostureLibrary.loadPosture("./postures/T_JOSE2");
            Gesture.Gesture gesture = new Gesture.Gesture("Gesto 1",PosturesOfGesture,"gesto1",2);
          
            Assert.IsTrue(gesLib.storeGesture(gesture));

            // primer paso. GestureLibray y gesture no compila porque no existen.
            // segundo paso. hago el codigo minimo para que compile, el metodo storegesture devuelve false por lo que veo al test fallar
            // tercer paso. Implemento el codigo de la funcion para que escriba el documento.
            // cuarto paso. tras multiples errores de memoria y nodos xml. el test pasa la prueba.
        }

        /*Metodo que va a porbar si cargamos un xml de gestos con su coleccion de posturas*/
        [Test]
        public void TestLoadGestureFromXML()
        {
            GestureLibrary gesLib = new GestureLibrary();
            Gesture.Gesture gesture = gesLib.loadGesture("Gesto 1");

            Assert.AreEqual("Gesto 1",gesture.name);
            Assert.AreEqual("T_JOSE", gesture.postures[0].name);
            Assert.AreEqual("T_JOSE2", gesture.postures[1].name);

            // primer paso.  no compila porque loadGesture no existe.
            // segundo paso. hago el codigo minimo para que compile, el metodo loadGesture devuelve un objeto null por lo que veo al test fallar
            // tercer paso. Implemento el codigo de la funcion para que recoja el gesture del docummento.
            // cuarto paso. sin errores en el codigo de la funcion,  el test pasa la prueba.
        }



    }
}
