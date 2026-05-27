using System;
using System.Data.Common;

namespace CastigoYedra.BBDD
{
    public class ConectorBD_MySQL : ConectorBD
    {
        private string urlServidor;
        private int puerto;
        private string usuario;
        private string password;

        private const string SCRIPT_BORRAR = "\\scripts\\borrarBD.sql";
        private const string SCRIPT_CREAR = "\\scripts\\crearBD.sql";

        public ConectorBD_MySQL(string nombreBD, string urlServidor, int puerto, string usuario, string password)
        {
            this.nombreBD = nombreBD;
            this.urlServidor = urlServidor;
            this.puerto = puerto;
            this.usuario = usuario;
            this.password = password;
            this.con = new MySql.Data.MySqlClient.MySqlConnection();
            this.con.ConnectionString = "server=" + urlServidor + ";Port=" + puerto + ";uid=" + usuario + ";pwd=" + password + ";database=" + nombreBD;
            System.Console.WriteLine("Cadena conexión: " + this.con.ConnectionString);
            this.con.Open();

            this.consulta = this.con.CreateCommand();
            this.tipo = ConectorBD.TIPO_MYSQL;
        }

        public override void borrarBD()
        {
            Console.WriteLine("Directorio actual: " + Environment.CurrentDirectory);

            string path = Environment.CurrentDirectory + SCRIPT_BORRAR;
            Console.WriteLine("Script a ejecutar: " + path);
            this.ejecutaScript(path);
        }

        public override void crearBD()
        {
            Console.WriteLine("Directorio actual: " + Environment.CurrentDirectory);
            string path = Environment.CurrentDirectory + SCRIPT_CREAR;
            Console.WriteLine("Script a ejecutar: " + path);
            this.ejecutaScript(path);
        }

        public override DbDataAdapter getAdapter(DbCommand consulta)
        {
            throw new NotImplementedException();
        }
    }
}