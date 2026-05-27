using CastigoYedra.BBDD;
using CastigoYedra.Clases;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pausePanel;

    bool pausado = false;

    void Awake()
    {
        // Singleton para acceso global al menú de pausa
        instance = this;
    }

    void Update()
    {
        // Alterna pausa con la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
                Reanudar();
            else
                Pausar();
        }
    }

    void Pausar()
    {
        // Activamos menú de pausa
        pausePanel.SetActive(true);

        // Pausamos el juego
        Time.timeScale = 0;

        // Permitimos interacción con el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Bloqueamos movimiento del jugador
        PlayerMovement.puedeMoverse = false;

        pausado = true;
    }

    public void Reanudar()
    {
        // Cerramos menú de pausa
        pausePanel.SetActive(false);

        // Reanudamos el tiempo del juego
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement.puedeMoverse = true;

        pausado = false;
    }

    public void GuardarPartida()
    {
        // Comprobamos conexión antes de guardar
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            // Guardamos el estado actual de la partida en BD
            SaveGameManager.instance.GuardarPartida();

            UIManager.instance.MostrarMensaje("Partida guardada con éxito");

            // Cerramos pausa tras guardar
            pausePanel.SetActive(false);
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PlayerMovement.puedeMoverse = true;

            pausado = false;
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al guardar la partida");
        }
    }

    public void CargarPartida()
    {
        // Acceso al menú de carga de partidas
        SceneManager.LoadScene("CargarPartida");
    }

    public void SalirMenu()
    {
        // Vuelta al menú principal
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirJuego()
    {
        // Cierra la aplicación (solo funciona en build)
        Application.Quit();
    }
}