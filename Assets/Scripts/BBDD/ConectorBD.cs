using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;

namespace CastigoYedra.BBDD
{
    public abstract class ConectorBD
    {
        public const string TIPO_MYSQL = "MySQL";
        public const string TIPO_SQLITE = "SQLITE";

        protected string nombreBD;
        protected DbConnection con;
        public DbCommand consulta;
        public string tipo;

        public static ConectorBD crearConectorBD(string tipo, string nombreBD, string urlServidor, int puerto, string usuario, string password)
        {
            switch (tipo)
            {
                case TIPO_MYSQL:
                    return new ConectorBD_MySQL(nombreBD, urlServidor, puerto, usuario, password);
                case TIPO_SQLITE:
                    return new ConectorBD_SQLite(nombreBD);
                default:
                    throw new Exception("Tipo de BD no soportado");
            }
        }

        public void cerrar()
        {
            if (con != null && con.State != System.Data.ConnectionState.Closed)
                con.Close();
        }

        public DbConnection getCon()
        {
            return con;
        }

        abstract public void crearBD();
        /**
         * Borra todo el contenido y las tablas de la BD
         * @throws java.lang.Exception
         */
        abstract public void borrarBD();

        /**
         * Borra y crea de nuevo toda la base de datos

         * @throws java.lang.Exception
         */
        public void reiniciarBD()
        {
            this.borrarBD();
            this.crearBD();
        }


        public void ejecutaScript(string fichero)
        {
            string script = File.ReadAllText(fichero);//@"E:\someSqlScript.sql"
            /*this.consulta.CommandText = script;
            Console.WriteLine("Linea a ejecutar: " + script);
            this.consulta.ExecuteNonQuery();
            */
            // split script on GO command
            IEnumerable<string> commandStrings = Regex.Split(script, @";$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string commandString in commandStrings)
            {
                if (!string.IsNullOrWhiteSpace(commandString.Trim()))
                {
                    this.consulta.CommandText = commandString;
                    Console.WriteLine("Linea a ejecutar: " + commandString);
                    this.consulta.ExecuteNonQuery();
                }
            }



        }
        /**
         * Obtiene un DataAdapter según el tipo de conector.
         * */
        public abstract DbDataAdapter getAdapter(DbCommand consulta);

        public void AddParameterWithValue(string parameterName, object value)
        {
            var parameter = consulta.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            consulta.Parameters.Add(parameter);
        }

        public void ClearParameters()
        {
            consulta.Parameters.Clear();
        }
    }
}
