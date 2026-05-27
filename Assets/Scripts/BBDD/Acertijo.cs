using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{
    internal class Acertijo : DBObject
    {
        protected const string ACERTIJO = "acertijo";
        protected const string ID = "idAcertijo";
        protected const string NOMBRE = "nombre";
        protected const string TIPO = "tipo";
        protected const string DESCRIPCION = "descripcion";

        public int idAcertijo { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }

        public Acertijo()
        {
            this.NombreTabla = ACERTIJO;
            this.Select = "SELECT * FROM " + ACERTIJO;
        }

        public override int readObject(DbDataReader rs)
        {
            this.idAcertijo = rs.GetInt32(rs.GetOrdinal(ID));
            this.nombre = rs.GetString(rs.GetOrdinal(NOMBRE));
            this.tipo = rs.GetString(rs.GetOrdinal(TIPO));
            this.descripcion = rs.GetString(rs.GetOrdinal(DESCRIPCION));

            return 0;
        }

        public override object readNewObject(DbDataReader rs)
        {
            Acertijo a = new Acertijo();
            a.readObject(rs);
            return a;
        }

        public override int deleteObject(ConectorBD con) { return 0; }
        public override void setValueAt(int index, object value) { }
        public override int writeNewObject(ConectorBD con) { return 0; }
        public override int writeObject(ConectorBD con) { return 0; }
    }
}