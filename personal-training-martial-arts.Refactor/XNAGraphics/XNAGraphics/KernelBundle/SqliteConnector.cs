using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace XNAGraphics.KernelBundle
{
    class SqliteConnector
    {
        SQLiteConnection conexion;
        public SqliteConnector(){
            conexion = new SQLiteConnection("Data Source=postures/posturesDB.s3db;Version=3;New=False;Compress=True;");
            conexion.Open();
        }

        public string insert(string consulta)
        {
            try
            {
                //throw new Exception(consulta);
                SQLiteCommand cmd = new SQLiteCommand(consulta, conexion);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return ("Error SQLite" + e);
            }
            return "";
        }

        public string postureGetId(string name)
        {
            string result = "";
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT ID FROM POSTURE WHERE NAME = '" + name + "'" , conexion);
                SQLiteDataReader datos = cmd.ExecuteReader();
                // Leemos los datos de forma repetitiva

                while (datos.Read())
                {
                    result = Convert.ToString(datos[0]);
                    // Y los mostramos
                }
            }
            catch (Exception e)
            {
                return ("");
            }
            return result;
        }

        public List<string> postureGetNames()
        {
            List<string> result = new List<string>();
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT NAME FROM POSTURE", conexion);
                SQLiteDataReader datos = cmd.ExecuteReader();
                // Leemos los datos de forma repetitiva

                while (datos.Read())
                {
                    result.Add(Convert.ToString(datos[0]));
                    // Y los mostramos
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return result;
        }

        public Tuple<string,string,string> postureGetData(string name)
        {
            Tuple<string, string, string> result = null;
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT NAME,DESCRIPTION,DIFFICULTY FROM POSTURE WHERE NAME = '" + name + "'", conexion);
                SQLiteDataReader datos = cmd.ExecuteReader();
                // Leemos los datos de forma repetitiva

                while (datos.Read())
                {
                    result = new Tuple<string, string, string>(Convert.ToString(datos[0]), Convert.ToString(datos[1]), Convert.ToString(datos[2]));
                    // Y los mostramos
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return result;
        }

        public List<Tuple<string, string, string, string>> postureGetJoints(string id)
        {
            List<Tuple<string, string, string, string>> result = new List<Tuple<string, string, string, string>>();
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("SELECT JOINT_ORDER, X, Y, Z FROM JOINT WHERE ID_POSTURE = " + id, conexion);
                SQLiteDataReader datos = cmd.ExecuteReader();
                // Leemos los datos de forma repetitiva

                while (datos.Read())
                {
                    result.Add(new Tuple<string, string, string, string>(Convert.ToString(datos[0]), Convert.ToString(datos[1]), Convert.ToString(datos[2]), Convert.ToString(datos[3])));
                    // Y los mostramos
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return result;
        }

        public bool CloseConnection(){
            conexion.Close();
            return true;
        }
    }
}
