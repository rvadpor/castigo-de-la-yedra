using System;
using System.Collections.Generic;
using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{
    internal class Inventario : DBObject
    {
        protected const string INVENTARIO = "inventario";
        protected const string ID = "idInventario";
        protected const string TAMANIO = "tamanio";

        public int idInventario { get; set; }
        public int tamanio { get; set; }

        public Inventario()
        {
            this.NombreTabla = INVENTARIO;
            this.Select = "SELECT * FROM " + INVENTARIO;
        }

        public override int deleteObject(ConectorBD con)
        {
            try
            {
                DbCommand consulta = con.consulta;

                consulta.Parameters.Clear();

                consulta.CommandText = "DELETE FROM " + INVENTARIO + " WHERE " + ID + "=@idInventario";

                con.AddParameterWithValue("@idInventario", this.idInventario);

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
            Inventario inv = new Inventario();
            inv.readObject(rs);
            return inv;
        }

        public override int readObject(DbDataReader rs)
        {
            if (rs.IsDBNull(rs.GetOrdinal(ID)))
                this.idInventario = 0;
            else
                this.idInventario = rs.GetInt32(rs.GetOrdinal(ID));

            if (rs.IsDBNull(rs.GetOrdinal(TAMANIO)))
                this.tamanio = 0;
            else
                this.tamanio = rs.GetInt32(rs.GetOrdinal(TAMANIO));

            return 0;
        }

        public override void setValueAt(int index, object value)
        {
            switch (index)
            {
                case 1:
                    this.idInventario = (int)value;
                    break;

                case 2:
                    this.tamanio = (int)value;
                    break;

                default:
                    throw new Exception("Columna no encontrada en Inventario.");
            }
        }

        public override int writeNewObject(ConectorBD con)
        {
            try
            {
                DbCommand consulta = con.consulta;

                consulta.Parameters.Clear();

                consulta.CommandText = "INSERT INTO " + INVENTARIO + "(" + TAMANIO + ") VALUES (@tamanio)";

                con.AddParameterWithValue("@tamanio", this.tamanio);

                consulta.ExecuteNonQuery();

                consulta.CommandText = "SELECT LAST_INSERT_ID()";
                this.idInventario = Convert.ToInt32(consulta.ExecuteScalar());

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

                consulta.CommandText = "UPDATE " + INVENTARIO + " SET " + TAMANIO + "=@tamanio WHERE " + ID + "=@idInventario";

                con.AddParameterWithValue("@tamanio", this.tamanio);
                con.AddParameterWithValue("@idInventario", this.idInventario);

                consulta.ExecuteNonQuery();

                consulta.Parameters.Clear();

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public void anadirObjeto(ConectorBD con, int idObjeto)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText = "INSERT INTO objeto_inventario(idObjeto,idInventario) VALUES(@idObjeto,@idInventario)";

            con.AddParameterWithValue("@idObjeto", idObjeto);
            con.AddParameterWithValue("@idInventario", this.idInventario);

            consulta.ExecuteNonQuery();

            consulta.Parameters.Clear();
        }

        public List<object> getObjetosInventario(ConectorBD con, int idInventario)
        {
            List<object> lista = new List<object>();

            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText = "SELECT idObjeto FROM objeto_inventario WHERE idInventario=@inv";

            con.AddParameterWithValue("@inv", idInventario);

            DbDataReader reader = consulta.ExecuteReader();

            List<int> idsObjetos = new List<int>();

            while (reader.Read())
            {
                idsObjetos.Add(reader.GetInt32(0));
            }

            reader.Close();

            foreach (int idObj in idsObjetos)
            {
                consulta.Parameters.Clear();

                consulta.CommandText = "SELECT idObjeto, nombre FROM objeto WHERE idObjeto=@id";

                con.AddParameterWithValue("@id", idObj);

                reader = consulta.ExecuteReader();

                if (reader.Read())
                {
                    Objeto obj = new Objeto();

                    obj.idObjeto = reader.GetInt32(0);
                    obj.nombre = reader.GetString(1);

                    lista.Add(obj);
                }

                reader.Close();
            }

            return lista;
        }
    }
}
