using CastigoYedra.BBDD;
using CastigoYedra.Clases;
using CastigoYedra.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionarJugadorUI : MonoBehaviour
{
    public Transform contenedorJugadores;
    public GameObject botonJugadorPrefab;

    void Start()
    {
        // Cargamos la lista de jugadores al entrar en la escena
        CargarJugadores();
    }

    void CargarJugadores()
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

            Jugador jugador = new Jugador();

            // Obtenemos todos los jugadores de la base de datos
            List<object> lista = jugador.readObjects(con);

            foreach (object obj in lista)
            {
                Jugador j = (Jugador)obj;

                CrearBotonJugador(j);
            }
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al cargar jugadores");
        }
    }

    void CrearBotonJugador(Jugador j)
    {
        GameObject boton = Instantiate(botonJugadorPrefab, contenedorJugadores);

        // Mostramos el nombre del jugador en el botón
        TMP_Text texto = boton.GetComponentInChildren<TMP_Text>();
        texto.text = j.nombre;

        // Cargamos la imagen del jugador desde el servidor
        Image imagenJugador = boton.transform.Find("ImagenJugador").GetComponent<Image>();

        StartCoroutine(CargarImagen(j.imagen, imagenJugador));

        // Al pulsar el botón seleccionamos ese jugador
        boton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            SeleccionarJugador(j);
        });
    }

    IEnumerator CargarImagen(string rutaRemota, Image imagenUI)
    {
        // Ruta temporal local para descargar la imagen
        string rutaLocal =Application.temporaryCachePath +"/" +Path.GetFileName(rutaRemota);

        // Descargamos la imagen desde el servidor
        DBManager.instance.sftp.download(rutaRemota, rutaLocal);

        yield return null;

        // Si el archivo existe, lo convertimos en sprite
        if (File.Exists(rutaLocal))
        {
            Texture2D textura = IOConnector.LoadTexture(rutaLocal);

            Sprite sprite = Sprite.Create(textura,new Rect(0, 0, textura.width, textura.height),new Vector2(0.5f, 0.5f));

            imagenUI.sprite = sprite;
        }
    }

    void SeleccionarJugador(Jugador j)
    {
        // Guardamos el jugador seleccionado en el GameManager
        GameManager.instance.idJugadorActual = j.idJugador;
        GameManager.instance.nombreJugador = j.nombre;

        Debug.Log("Jugador seleccionado: " + j.nombre);

        // Pasamos al menú del jugador
        SceneManager.LoadScene("MenuJugador");
    }

    public void Volver()
    {
        // Volvemos al menú principal
        SceneManager.LoadScene("MainMenu");
    }
}