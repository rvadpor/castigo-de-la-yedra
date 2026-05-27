using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using UnityEngine;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class LoadGameManager : MonoBehaviour
{
    public Transform jugador;
    public GameObject enemigo;

    void Start()
    {
        // Se usa una corrutina para asegurar que todo está inicializado antes de cargar datos
        StartCoroutine(LoadRoutine());
    }

    IEnumerator LoadRoutine()
    {
        // Espera 1 frame para evitar problemas de inicialización
        yield return null;

        // Cargamos el estado guardado de la partida
        CargarPosicionJugador();
        CargarInventario();

        // Si el jugador ha recogido el objeto 3, activamos el enemigo y su música
        if (WorldStateManager.instance.ObjetoRecogido(3))
        {
            enemigo.SetActive(true);
            AudioManager.instance.ReproducirMusicaEnemigo();
        }

        // Bloqueamos/activamos controles del jugador al iniciar la escena
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement.puedeMoverse = true;
        Time.timeScale = 1f;
    }

    void CargarPosicionJugador()
    {
        ConectorBD con = DBManager.instance.conexion;
        DbCommand consulta = con.consulta;

        consulta.Parameters.Clear();

        // Obtenemos la posición guardada de la partida actual
        consulta.CommandText ="SELECT partidaGuardada FROM partida WHERE idPartida=@id";

        con.AddParameterWithValue("@id", GameManager.instance.idPartidaActual);

        DbDataReader reader = consulta.ExecuteReader();

        if (reader.Read())
        {
            string datos = reader.GetString(0);

            if (!string.IsNullOrEmpty(datos))
            {
                // Convertimos el string guardado en posición Vector3
                string[] pos = datos.Split(',');

                float x = float.Parse(pos[0], CultureInfo.InvariantCulture);
                float y = float.Parse(pos[1], CultureInfo.InvariantCulture);
                float z = float.Parse(pos[2], CultureInfo.InvariantCulture);

                Vector3 posicion = new Vector3(x, y, z);

                jugador.position = posicion;

                Debug.Log("Posición cargada: " + posicion);
            }
        }

        reader.Close();
    }

    void CargarInventario()
    {
        ConectorBD con = DBManager.instance.conexion;

        int idInventario = GameManager.instance.idInventarioActual;

        Inventario inv = new Inventario();

        // Obtenemos los objetos del inventario desde la base de datos
        List<object> lista = inv.getObjetosInventario(con, idInventario);

        foreach (object obj in lista)
        {
            Objeto o = (Objeto)obj;

            Item item = new Item();
            item.id = o.idObjeto;
            item.nombre = o.nombre;

            // Añadimos los items al inventario del jugador sin mostrar mensaje
            InventoryManager.instance.AñadirItem(item, false);
        }

        Debug.Log("Inventario cargado");
    }
}