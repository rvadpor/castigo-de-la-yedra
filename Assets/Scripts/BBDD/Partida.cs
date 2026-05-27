using System;
using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{

    public class Partida : DBObject
    {
        protected const string PARTIDA = "partida";

        protected const string ID_PARTIDA = "idPartida";
        protected const string PARTIDA_GUARDADA = "partidaGuardada";
        protected const string ID_JUGADOR = "idJugador";
        protected const string ID_INVENTARIO = "idInventario";

        public int idPartida { get; set; }
        public string partidaGuardada { get; set; }
        public int idJugador { get; set; }
        public int idInventario { get; set; }

        public Partida()
        {
            this.NombreTabla = PARTIDA;
            this.Select = "SELECT * FROM " + PARTIDA;
        }

        public override int writeNewObject(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText ="INSERT INTO partida (partidaGuardada, idJugador, idInventario) VALUES (@partidaGuardada, @idJugador, @idInventario)";

            con.AddParameterWithValue("@partidaGuardada", this.partidaGuardada);
            con.AddParameterWithValue("@idJugador", this.idJugador);
            con.AddParameterWithValue("@idInventario", this.idInventario);

            consulta.ExecuteNonQuery();

            consulta.CommandText = "SELECT LAST_INSERT_ID()";
            this.idPartida = Convert.ToInt32(consulta.ExecuteScalar());

            consulta.Parameters.Clear();

            return 0;
        }

        public override int writeObject(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText ="UPDATE partida SET partidaGuardada=@partidaGuardada WHERE idPartida=@idPartida";

            con.AddParameterWithValue("@partidaGuardada", this.partidaGuardada);
            con.AddParameterWithValue("@idPartida", this.idPartida);

            consulta.ExecuteNonQuery();

            consulta.Parameters.Clear();

            return 0;
        }

        public override int deleteObject(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText ="DELETE FROM partida WHERE idPartida=@idPartida";

            con.AddParameterWithValue("@idPartida", this.idPartida);

            consulta.ExecuteNonQuery();

            consulta.Parameters.Clear();

            return 0;
        }

        public override int readObject(DbDataReader rs)
        {
            if (!rs.IsDBNull(rs.GetOrdinal(ID_PARTIDA)))
                this.idPartida = rs.GetInt32(rs.GetOrdinal(ID_PARTIDA));

            if (!rs.IsDBNull(rs.GetOrdinal(PARTIDA_GUARDADA)))
                this.partidaGuardada = rs.GetString(rs.GetOrdinal(PARTIDA_GUARDADA));

            if (!rs.IsDBNull(rs.GetOrdinal(ID_JUGADOR)))
                this.idJugador = rs.GetInt32(rs.GetOrdinal(ID_JUGADOR));

            if (!rs.IsDBNull(rs.GetOrdinal(ID_INVENTARIO)))
                this.idInventario = rs.GetInt32(rs.GetOrdinal(ID_INVENTARIO));

            return 0;
        }

        public override object readNewObject(DbDataReader rs)
        {
            Partida p = new Partida();
            p.readObject(rs);
            return p;
        }

        public override void setValueAt(int index, object value)
        {
            switch (index)
            {
                case 1:
                    this.idPartida = (int)value;
                    break;
                case 2:
                    this.partidaGuardada = value.ToString();
                    break;
                case 3:
                    this.idJugador = (int)value;
                    break;
                case 4:
                    this.idInventario = (int)value;
                    break;
                default:
                    throw new Exception("Columna no encontrada en Partida");
            }
        }

        public bool ExistePartidaJugador(ConectorBD con, int idJugador)
        {
            con.consulta.Parameters.Clear();

            con.consulta.CommandText ="SELECT COUNT(*) FROM partida WHERE idJugador=@idJugador";

            con.AddParameterWithValue("@idJugador", idJugador);

            int count = Convert.ToInt32(con.consulta.ExecuteScalar());

            con.consulta.Parameters.Clear();

            return count > 0;
        }
    }
}
