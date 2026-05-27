using System;
using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{
    internal class PartidaAcertijo
    {
        public int idPartida { get; set; }
        public int idAcertijo { get; set; }
        public bool resuelto { get; set; }

        public void marcarResuelto(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText ="UPDATE partida_acertijo SET resuelto=TRUE WHERE idPartida=@idPartida AND idAcertijo=@idAcertijo";

            con.AddParameterWithValue("@idPartida", this.idPartida);
            con.AddParameterWithValue("@idAcertijo", this.idAcertijo);

            consulta.ExecuteNonQuery();

            consulta.Parameters.Clear();
        }

        public bool estaResuelto(ConectorBD con)
        {
            DbCommand consulta = con.consulta;

            consulta.Parameters.Clear();

            consulta.CommandText ="SELECT resuelto FROM partida_acertijo WHERE idPartida=@idPartida AND idAcertijo=@idAcertijo";

            con.AddParameterWithValue("@idPartida", this.idPartida);
            con.AddParameterWithValue("@idAcertijo", this.idAcertijo);

            object result = consulta.ExecuteScalar();

            consulta.Parameters.Clear();

            if (result == null)
                return false;

            return Convert.ToBoolean(result);
        }
    }
}