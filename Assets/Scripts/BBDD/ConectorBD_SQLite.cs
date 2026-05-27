using System;
using System.Data.Common;
using System.Data.SQLite;
using UnityEngine;

namespace CastigoYedra.BBDD
{
    public class ConectorBD_SQLite : ConectorBD
    {
        private const string SCRIPT_BORRAR = "/scripts/borrarBD_SQLITE.sql";
        private const string SCRIPT_CREAR = "/scripts/crearBD_SQLITE.sql";

        public ConectorBD_SQLite(string nombreBD)
        {
            this.nombreBD = nombreBD;


            string dbPath = Application.persistentDataPath + "/" + nombreBD + ".db";
            string connStr = $"Data Source={dbPath};Version=3;New=True;Compress=True;";
            this.con = new System.Data.SQLite.SQLiteConnection(connStr);

            try
            {
                this.con.Open();
                this.consulta = this.con.CreateCommand();
                this.tipo = ConectorBD.TIPO_SQLITE;
            }
            catch (Exception e)
            {
                Debug.LogError("Error al abrir SQLite: " + e.Message);
            }
        }

        public override void borrarBD()
        {
            string path = Application.dataPath + SCRIPT_BORRAR;
            Debug.Log("Ejecutando script borrar: " + path);
            this.ejecutaScript(path);
        }

        public override void crearBD()
        {
            string path = Application.dataPath + SCRIPT_CREAR;
            Debug.Log("Ejecutando script crear: " + path);
            this.ejecutaScript(path);
        }

        public override DbDataAdapter getAdapter(DbCommand consulta)
        {
            return new SQLiteDataAdapter((System.Data.SQLite.SQLiteCommand)consulta);
        }
    }
}