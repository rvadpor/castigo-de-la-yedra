using System;
using System.Collections.Generic;
using System.Data.Common;
using CastigoYedra.BBDD;

namespace CastigoYedra.Clases
{
    public interface SQLParsable
    {
        /**
    * Lee los valores de un registro del resultset y se los asigna al objeto
    * actual
    *
    * @param rs ResultSet a utilizar. Debe estar inicializado (rs.next())
    * @return Devuelve 0 si todo es correcto o un valor negativo en caso de
    * error.
    * @throws SQLException Si hay error lanza la excepción pertinente
    */
        int readObject(DbDataReader rs);

        /**
         * Modifica el registro correspondiente de la BD con lo valores de este
         * objeto. Update.
         *
         * @param con Conexión a utilizar.
         * @return Devuelve 0 si todo es correcto o un valor negativo en caso de
         * error.
         * @throws SQLException SQLException Si hay error lanza la excepción
         * pertinente
         */
        int writeObject(ConectorBD con);

        /**
         * Crea un nuevo registro en la BD con los datos del objeto.
         *
         * @param con Conexión a utilizar.
         * @return Devuelve 0 si todo es correcto o un valor negativo en caso de
         * error.
         * @throws SQLException Si hay error lanza la excepción pertinente
         */
        int writeNewObject(ConectorBD con);

        /**
         * Crea un objeto con los datos del registro del resultset.
         *
         * @param rs ResultSet a utilizar. Debe estar inicializado (rs.next())
         * @return Devuelve 0 si todo es correcto o un valor negativo en caso de
         * error.
         * @throws SQLException Si hay error lanza la excepción pertinente
         */
        Object readNewObject(DbDataReader rs);

        /**
         * Obtiene todos los objetos de este tipo de la base de datos.
         * @param con
         * @return
         * @throws SQLException 
         */
        List<Object> readObjects(ConectorBD con);

        /**
         * Elimina de la base de datos el objeto en cuestión. Esto requerirá eliminar el objeto a posteriori.
         * @param con Conector de la base de datos.
         * @return Devuelve 0 si no hay errores o un número negativo en caso de error.
         */
        int deleteObject(ConectorBD con);
        /**
         * Establece el valor de un campo según el índice
         * @param index: índice del campo a establecer.
         * @param value: Valor del campo a establecer.
         */
        void setValueAt(int index, Object value);

        /**
         * Obtiene el siguiente ID para la clave primaria.
         */
        int getNextId(ConectorBD con);

    }
}
