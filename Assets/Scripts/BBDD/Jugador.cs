using System;
using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{
    public class Jugador : DBObject
    {
        protected const string TABLA = "jugador";
        protected const string ID = "idJugador";
        protected const string NOMBRE = "nombre";
        protected const string IMAGEN = "imagen";

        public int idJugador { get; set; }
        public string nombre { get; set; }
        public string imagen { get; set; }

        public Jugador()
        {
            this.NombreTabla = TABLA;
            this.Select = "SELECT * FROM " + TABLA;
        }

        public override object readNewObject(DbDataReader rs)
        {
            Jugador j = new Jugador();
            j.readObject(rs);
            return j;
        }

        public override int readObject(DbDataReader rs)
        {
            if (!rs.IsDBNull(rs.GetOrdinal(ID)))
                this.idJugador = rs.GetInt32(rs.GetOrdinal(ID));

            if (!rs.IsDBNull(rs.GetOrdinal(NOMBRE)))
                this.nombre = rs.GetString(rs.GetOrdinal(NOMBRE));

            if (!rs.IsDBNull(rs.GetOrdinal("imagen")))
                this.imagen = rs.GetString(rs.GetOrdinal("imagen"));

            return 0;
        }

        public override int writeNewObject(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();
            consulta.CommandText = "INSERT INTO jugador(nombre, imagen) VALUES(@nombre, @imagen)";

            con.AddParameterWithValue("@nombre", this.nombre);
            con.AddParameterWithValue("@imagen", "/home/rvadpor116/imagenes/jugador.png");

            consulta.ExecuteNonQuery();

            this.idJugador = getNextId(con);

            consulta.Parameters.Clear();

            return 0;
        }

        public override int writeObject(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();
            consulta.CommandText = "UPDATE jugador SET nombre=@nombre, imagen=@imagen WHERE idJugador=@id";

            con.AddParameterWithValue("@nombre", this.nombre);
            con.AddParameterWithValue("@imagen", this.imagen);
            con.AddParameterWithValue("@id", this.idJugador);

            consulta.ExecuteNonQuery();

            consulta.Parameters.Clear();

            return 0;
        }

        public override int deleteObject(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();
            consulta.CommandText = "DELETE FROM jugador WHERE idJugador=@id";

            con.AddParameterWithValue("@id", this.idJugador);

            consulta.ExecuteNonQuery();

            consulta.Parameters.Clear();

            return 0;
        }

        public override void setValueAt(int index, object value)
        {
            switch (index)
            {
                case 1:
                    this.idJugador = (int)value;
                    break;

                case 2:
                    this.nombre = value.ToString();
                    break;
                case 3:
                    this.imagen = value.ToString();
                    break;

                default:
                    throw new Exception("Columna no válida en Jugador");
            }
        }
    }
}