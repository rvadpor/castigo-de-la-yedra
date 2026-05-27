using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class CargarPartidaUI : MonoBehaviour
{
    public Transform contenedorPartidas;
    public GameObject botonPartidaPrefab;

    void Start()
    {
        // Al entrar en la escena cargamos las partidas disponibles
        CargarPartidas();
    }

    void CargarPartidas()
    {
        // Comprobamos conexión con la base de datos
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            ConectorBD con = DBManager.instance.conexion;

            // Obtenemos todas las partidas de la base de datos
            Partida partida = new Partida();
            List<object> lista = partida.readObjects(con);

            // Filtramos solo las partidas del jugador actual
            foreach (object obj in lista)
            {
                Partida p = (Partida)obj;

                if (p.idJugador == GameManager.instance.idJugadorActual)
                    CrearBotonPartida(p);
            }
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al cargar partidas");
        }
    }

    void CrearBotonPartida(Partida p)
    {
        // Creamos botón dinámico por cada partida
        GameObject boton = Instantiate(botonPartidaPrefab, contenedorPartidas);

        boton.GetComponentInChildren<TMP_Text>().text = "Partida " + p.idPartida;

        // Al pulsar cargamos esa partida
        boton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CargarPartida(p));
    }

    void CargarPartida(Partida p)
    {
        // Guardamos el estado de la partida en GameManager para usarlo en otras escenas
        GameManager.instance.idPartidaActual = p.idPartida;
        GameManager.instance.idInventarioActual = p.idInventario;

        Debug.Log("Partida cargada: " + p.idPartida);

        // Cambiamos a la escena de juego
        SceneManager.LoadScene("Juego");
    }

    public void Volver()
    {
        SceneManager.LoadScene("MenuJugador");
    }
}