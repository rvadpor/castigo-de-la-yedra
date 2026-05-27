using System;
using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{
    internal class Objeto : DBObject
    {
        protected const string OBJETO = "objeto";
        protected const string ID = "idObjeto";
        protected const string NOMBRE = "nombre";
        protected const string TIPO = "tipo";

        public int idObjeto { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }

        public Objeto()
        {
            this.NombreTabla = OBJETO;
            this.Select = "SELECT * FROM " + OBJETO;
        }

        public override int deleteObject(ConectorBD con)
        {
            try
            {
                DbCommand consulta = con.consulta;

                consulta.Parameters.Clear();

                consulta.CommandText ="DELETE FROM " + OBJETO + " WHERE " + ID + "=@idObjeto";

                con.AddParameterWithValue("@idObjeto", this.idObjeto);

                consulta.ExecuteNonQuery();

                consulta.Parameters.Clear();

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override object readNewObject(DbDataReader rs)
        {
            Objeto obj = new Objeto();
            obj.readObject(rs);
            return obj;
        }

        public override int readObject(DbDataReader rs)
        {
            if (rs.IsDBNull(rs.GetOrdinal(ID)))
                this.idObjeto = 0;
            else
                this.idObjeto = rs.GetInt32(rs.GetOrdinal(ID));

            if (rs.IsDBNull(rs.GetOrdinal(NOMBRE)))
                this.nombre = null;
            else
                this.nombre = rs.GetString(rs.GetOrdinal(NOMBRE));

            if (rs.IsDBNull(rs.GetOrdinal(TIPO)))
                this.tipo = null;
            else
                this.tipo = rs.GetString(rs.GetOrdinal(TIPO));

            return 0;
        }

        public override void setValueAt(int index, object value)
        {
            switch (index)
            {
                case 1:
                    this.idObjeto = (int)value;
                    break;

                case 2:
                    this.nombre = value.ToString();
                    break;

                case 3:
                    this.tipo = value.ToString();
                    break;

                default:
                    throw new Exception("Columna no encontrada en Objeto.");
            }
        }

        public override int writeNewObject(ConectorBD con)
        {
            try
            {
                DbCommand consulta = con.consulta;

                consulta.Parameters.Clear();

                consulta.CommandText = "INSERT INTO " + OBJETO + "(" + NOMBRE + "," + TIPO + ") VALUES (@nombre,@tipo)";

                con.AddParameterWithValue("@nombre", this.nombre);
                con.AddParameterWithValue("@tipo", this.tipo);

                consulta.ExecuteNonQuery();

                consulta.CommandText = "SELECT LAST_INSERT_ID()";
                this.idObjeto = Convert.ToInt32(consulta.ExecuteScalar());

                consulta.Parameters.Clear();

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public override int writeObject(ConectorBD con)
        {
            try
            {
                DbCommand consulta = con.consulta;

                consulta.Parameters.Clear();

                consulta.CommandText ="UPDATE " + OBJETO + " SET " +NOMBRE + "=@nombre," +TIPO + "=@tipo " +"WHERE " + ID + "=@idObjeto";

                con.AddParameterWithValue("@nombre", this.nombre);
                con.AddParameterWithValue("@tipo", this.tipo);
                con.AddParameterWithValue("@idObjeto", this.idObjeto);

                consulta.ExecuteNonQuery();

                consulta.Parameters.Clear();

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        public override string ToString()
        {
            return "[Objeto: " + this.idObjeto + "," + this.nombre + "," + this.tipo + "]";
        }
    }
}
