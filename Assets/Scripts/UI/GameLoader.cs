using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class GameLoader : MonoBehaviour
{
    public Transform jugador;

    void Start()
    {
        // Al iniciar la escena de juego se carga todo el estado guardado
        CargarPosicionJugador();
        CargarInventario();
        CargarAcertijos();
    }

    void CargarPosicionJugador()
    {
        ConectorBD con = DBManager.instance.conexion;

        // Leemos todas las partidas de la BD
        Partida p = new Partida();
        List<object> lista = p.readObjects(con);

        foreach (object obj in lista)
        {
            Partida partida = (Partida)obj;

            // Buscamos la partida actual
            if (partida.idPartida == GameManager.instance.idPartidaActual)
            {
                // Si existe posición guardada la aplicamos al jugador
                if (!string.IsNullOrEmpty(partida.partidaGuardada))
                {
                    string[] pos = partida.partidaGuardada.Split(',');

                    Vector3 posicion = new Vector3(float.Parse(pos[0]),float.Parse(pos[1]),float.Parse(pos[2]));

                    jugador.position = posicion;
                }
            }
        }
    }

    void CargarInventario()
    {
        ConectorBD con = DBManager.instance.conexion;
        DbCommand consulta = con.consulta;

        consulta.Parameters.Clear();

        // Consulta para obtener los objetos del inventario actual
        consulta.CommandText ="SELECT objeto.idObjeto, objeto.nombre FROM objeto JOIN objeto_inventario ON objeto.idObjeto = objeto_inventario.idObjeto WHERE idInventario = @idInventario";

        con.AddParameterWithValue("@idInventario", GameManager.instance.idInventarioActual);

        var reader = consulta.ExecuteReader();

        // Creamos los items en memoria a partir de la BD
        while (reader.Read())
        {
            Item item = new Item();
            item.id = reader.GetInt32(0);
            item.nombre = reader.GetString(1);

            InventoryManager.instance.AñadirItem(item);
        }

        reader.Close();
    }

    void CargarAcertijos()
    {
        ConectorBD con = DBManager.instance.conexion;
        DbCommand consulta = con.consulta;

        consulta.Parameters.Clear();

        // Obtenemos los acertijos resueltos de la partida actual
        consulta.CommandText ="SELECT idAcertijo FROM partida_acertijo WHERE idPartida=@idPartida AND resuelto=true";

        con.AddParameterWithValue("@idPartida", GameManager.instance.idPartidaActual);

        var reader = consulta.ExecuteReader();

        // Marcamos como resueltos los puzzles en el juego
        while (reader.Read())
        {
            int idAcertijo = reader.GetInt32(0);
            PuzzleManager.instance.MarcarResuelto(idAcertijo);
        }

        reader.Close();
    }
}