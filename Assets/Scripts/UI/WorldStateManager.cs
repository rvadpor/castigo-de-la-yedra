using UnityEngine;
using System.Collections.Generic;
using System.Data.Common;
using CastigoYedra.BBDD;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager instance;

    // Lista de objetos que el jugador ya ha recogido en la partida
    List<int> objetosRecogidos = new List<int>();

    // Lista de acertijos que ya están resueltos en la partida
    List<int> acertijosResueltos = new List<int>();

    void Awake()
    {
        // Singleton para acceso global al estado del mundo
        instance = this;

        // Reiniciamos listas al iniciar la escena
        objetosRecogidos.Clear();
        acertijosResueltos.Clear();

        // Cargamos el estado guardado desde la base de datos
        CargarObjetosRecogidos();
        CargarAcertijosResueltos();
    }

    void CargarObjetosRecogidos()
    {
        // Comprobamos conexión antes de consultar la base de datos
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            ConectorBD con = DBManager.instance.conexion;

            DbCommand consulta = con.consulta;
            consulta.Parameters.Clear();

            consulta.CommandText ="SELECT idObjeto FROM objeto_inventario WHERE idInventario=@inv";

            con.AddParameterWithValue("@inv", GameManager.instance.idInventarioActual);

            DbDataReader reader = consulta.ExecuteReader();

            // Guardamos todos los objetos recogidos en memoria
            while (reader.Read())
            {
                objetosRecogidos.Add(reader.GetInt32(0));
            }

            reader.Close();
        }
        catch
        {
            UIMessage.instance.Mostrar("Error en la conexión");
        }
    }

    void CargarAcertijosResueltos()
    {
        // Comprobamos conexión antes de consultar la base de datos
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            ConectorBD con = DBManager.instance.conexion;

            DbCommand consulta = con.consulta;
            consulta.Parameters.Clear();

            consulta.CommandText = "SELECT idAcertijo FROM partida_acertijo WHERE idPartida=@p AND resuelto=1";

            con.AddParameterWithValue("@p", GameManager.instance.idPartidaActual);

            DbDataReader reader = consulta.ExecuteReader();

            // Guardamos acertijos resueltos en memoria
            while (reader.Read())
            {
                acertijosResueltos.Add(reader.GetInt32(0));
            }

            reader.Close();
        }
        catch
        {
            UIMessage.instance.Mostrar("Error en la conexión");
        }
    }

    // Devuelve si un objeto ya ha sido recogido
    public bool ObjetoRecogido(int id)
    {
        return objetosRecogidos.Contains(id);
    }

    // Devuelve si un acertijo ya ha sido resuelto
    public bool AcertijoResuelto(int id)
    {
        return acertijosResueltos.Contains(id);
    }
}