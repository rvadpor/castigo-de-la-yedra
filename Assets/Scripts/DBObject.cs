using System;
using System.Collections.Generic;
using System.Data.Common;
using CastigoYedra.BBDD;
using CastigoYedra.IO;

namespace CastigoYedra.Clases
{
    public abstract class DBObject : SQLParsable
    {

        public static IOConnector ioCon { get; set; } = null;


        public bool modificado { get; set; } = false;


        protected string NombreTabla;


        protected string Select;

        public abstract int deleteObject(ConectorBD con);
        public abstract object readNewObject(DbDataReader rs);
        public abstract int readObject(DbDataReader rs);
        public abstract void setValueAt(int index, object value);
        public abstract int writeNewObject(ConectorBD con);
        public abstract int writeObject(ConectorBD con);

        public virtual List<object> readObjects(ConectorBD con)
        {
            List<object> objs = new List<object>();
            DbCommand consulta = con.consulta;
            consulta.CommandText = this.Select;
            DbDataReader rs = consulta.ExecuteReader();
            while (rs.Read())
            {
                object obj = this.readNewObject(rs);
                objs.Add(obj);
            }
            rs.Close();
            return objs;
        }


        public int getNextId(ConectorBD con)
        {
            con.consulta.Parameters.Clear();
            con.consulta.CommandText = "SELECT LAST_INSERT_ID()";
            object o = con.consulta.ExecuteScalar();
            return Convert.ToInt32(o);
        }
    }
}