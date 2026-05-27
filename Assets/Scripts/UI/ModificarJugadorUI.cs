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

public class ModificarJugadorUI : MonoBehaviour
{
    public Transform contenedorJugadores;
    public GameObject botonJugadorPrefab;

    [Header("UI Modificar")]
    public GameObject panelModificar;
    public TMP_InputField inputNombre;
    public Image previewImagen;

    [Header("Selector Imagen")]
    public GameObject panelSelectorImagen;

    private Jugador jugadorSeleccionado;
    ConectorBD con;

    void Start()
    {
        // Obtenemos la conexión a la base de datos desde el singleton
        con = DBManager.instance.conexion;

        // Cargamos la lista de jugadores al iniciar la escena
        CargarJugadores();

        // Ocultamos paneles al inicio
        panelModificar.SetActive(false);
        panelSelectorImagen.SetActive(false);
    }

    void CargarJugadores()
    {
        // Comprobamos conexión antes de intentar leer la BD
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            // Limpiamos la lista anterior de botones
            foreach (Transform child in contenedorJugadores)
                Destroy(child.gameObject);

            // Leemos jugadores desde la base de datos
            Jugador jugador = new Jugador();
            List<object> lista = jugador.readObjects(con);

            // Creamos un botón por cada jugador
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
        // Instanciamos el prefab del botón en el contenedor
        GameObject boton = Instantiate(botonJugadorPrefab, contenedorJugadores);

        // Asignamos el nombre del jugador al texto del botón
        TMP_Text texto = boton.GetComponentInChildren<TMP_Text>();
        texto.text = j.nombre;

        // Cargamos la imagen del jugador desde el servidor
        Image imagenJugador = boton.transform.Find("ImagenJugador").GetComponent<Image>();
        StartCoroutine(CargarImagen(j.imagen, imagenJugador));

        // Evento click para abrir el panel de modificación
        boton.GetComponent<Button>().onClick.AddListener(() => AbrirModificacion(j));
    }

    IEnumerator CargarImagen(string rutaRemota, Image imagenUI)
    {
        // Descargamos la imagen temporalmente para poder cargarla en Unity
        string rutaLocal = Application.temporaryCachePath + "/" + Path.GetFileName(rutaRemota);

        DBManager.instance.sftp.download(rutaRemota, rutaLocal);

        yield return null;

        // Convertimos el archivo descargado en Sprite para la UI
        if (File.Exists(rutaLocal))
        {
            Texture2D textura = IOConnector.LoadTexture(rutaLocal);
            Sprite sprite = Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));
            imagenUI.sprite = sprite;
        }
    }

    void AbrirModificacion(Jugador j)
    {
        // Guardamos el jugador seleccionado para editarlo después
        jugadorSeleccionado = j;

        // Rellenamos el input con el nombre actual
        inputNombre.text = j.nombre;

        // Mostramos su imagen en el preview
        ActualizarPreview(j.imagen);

        panelModificar.SetActive(true);
    }

    public void AbrirSelectorImagen()
    {
        panelSelectorImagen.SetActive(true);
    }

    public void CerrarSelectorImagen()
    {
        panelSelectorImagen.SetActive(false);
    }

    public void CambiarImagen(string ruta)
    {
        // Si no hay jugador seleccionado no hacemos nada
        if (jugadorSeleccionado == null) return;

        // Guardamos la nueva ruta de la imagen
        jugadorSeleccionado.imagen = ruta;

        // Actualizamos la previsualización
        ActualizarPreview(ruta);

        panelSelectorImagen.SetActive(false);
    }

    public void ActualizarPreview(string rutaRemota)
    {
        StartCoroutine(CargarPreview(rutaRemota));
    }

    IEnumerator CargarPreview(string rutaRemota)
    {
        // Descarga temporal para mostrar la imagen en UI
        string rutaLocal = Application.temporaryCachePath + "/" + Path.GetFileName(rutaRemota);

        DBManager.instance.sftp.download(rutaRemota, rutaLocal);

        yield return null;

        if (File.Exists(rutaLocal))
        {
            Texture2D textura = IOConnector.LoadTexture(rutaLocal);

            Sprite sprite = Sprite.Create(textura,new Rect(0, 0, textura.width, textura.height),new Vector2(0.5f, 0.5f));

            previewImagen.sprite = sprite;
        }
    }

    public void ConfirmarCambio()
    {
        // Comprobamos conexión antes de guardar cambios
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            if (jugadorSeleccionado == null) return;

            string nuevoNombre = inputNombre.text.Trim();

            if (string.IsNullOrEmpty(nuevoNombre))
            {
                UIMessage.instance.Mostrar("Introduce un nombre");
                return;
            }

            // Actualizamos datos del jugador
            jugadorSeleccionado.nombre = nuevoNombre;

            // Guardamos cambios en base de datos
            jugadorSeleccionado.writeObject(con);

            UIMessage.instance.Mostrar("Jugador modificado correctamente");

            jugadorSeleccionado = null;
            panelModificar.SetActive(false);

            // Recargamos lista de jugadores
            CargarJugadores();
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al modificar jugador");
        }
    }

    public void Cancelar()
    {
        jugadorSeleccionado = null;
        panelModificar.SetActive(false);
    }

    public void Volver()
    {
        SceneManager.LoadScene("MainMenu");
    }
}