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

public class BorrarJugadorUI : MonoBehaviour
{
    public Transform contenedorJugadores;
    public GameObject botonJugadorPrefab;

    [Header("Confirmacion")]
    public GameObject panelConfirmacion;
    public TMP_Text textoConfirmacion;

    private Jugador jugadorSeleccionado;
    ConectorBD con;

    void Start()
    {
        // Obtenemos conexión a la base de datos
        con = DBManager.instance.conexion;

        // Cargamos lista de jugadores al iniciar
        CargarJugadores();

        // Ocultamos panel de confirmación
        panelConfirmacion.SetActive(false);
    }

    void CargarJugadores()
    {
        // Limpiamos botones anteriores
        foreach (Transform child in contenedorJugadores)
            Destroy(child.gameObject);

        // Leemos jugadores desde base de datos
        Jugador jugador = new Jugador();
        List<object> lista = jugador.readObjects(con);

        // Creamos un botón por cada jugador
        foreach (object obj in lista)
        {
            Jugador j = (Jugador)obj;
            CrearBotonJugador(j);
        }
    }

    void CrearBotonJugador(Jugador j)
    {
        // Instanciamos botón en el contenedor
        GameObject boton = Instantiate(botonJugadorPrefab, contenedorJugadores);

        // Asignamos nombre al texto del botón
        TMP_Text texto = boton.GetComponentInChildren<TMP_Text>();
        texto.text = j.nombre;

        // Cargamos imagen del jugador desde servidor
        Image imagenJugador = boton.transform.Find("ImagenJugador").GetComponent<Image>();
        StartCoroutine(CargarImagen(j.imagen, imagenJugador));

        // Evento click para mostrar confirmación de borrado
        boton.GetComponent<Button>().onClick.AddListener(() => MostrarConfirmacion(j));
    }

    IEnumerator CargarImagen(string rutaRemota, Image imagenUI)
    {
        // Descargamos imagen temporalmente para mostrarla en UI
        string rutaLocal = Application.temporaryCachePath + "/" + Path.GetFileName(rutaRemota);

        DBManager.instance.sftp.download(rutaRemota, rutaLocal);

        yield return null;

        // Convertimos archivo en sprite para Unity
        if (File.Exists(rutaLocal))
        {
            Texture2D textura = IOConnector.LoadTexture(rutaLocal);

            Sprite sprite = Sprite.Create(textura,new Rect(0, 0, textura.width, textura.height),new Vector2(0.5f, 0.5f));

            imagenUI.sprite = sprite;
        }
    }

    void MostrarConfirmacion(Jugador j)
    {
        // Guardamos jugador seleccionado
        jugadorSeleccionado = j;

        // Mostramos mensaje de confirmación
        textoConfirmacion.text = "¿Seguro que quieres borrar a " + j.nombre + "?";

        panelConfirmacion.SetActive(true);
    }

    public void ConfirmarBorrado()
    {
        // Comprobamos conexión antes de borrar
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            if (jugadorSeleccionado == null) return;

            int idJugador = jugadorSeleccionado.idJugador;

            // Eliminamos datos relacionados del jugador (partidas, inventario, etc.)
            BorrarDatosJugador(idJugador);

            // Eliminamos jugador de la base de datos
            jugadorSeleccionado.deleteObject(con);

            UIMessage.instance.Mostrar("Jugador " + jugadorSeleccionado.nombre + " borrado correctamente");

            jugadorSeleccionado = null;
            panelConfirmacion.SetActive(false);

            // Recargamos lista de jugadores
            CargarJugadores();
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al borrar jugador");
        }
    }

    public void CancelarBorrado()
    {
        jugadorSeleccionado = null;
        panelConfirmacion.SetActive(false);
    }

    void BorrarDatosJugador(int idJugador)
    {
        // Obtenemos partidas del jugador
        con.consulta.CommandText = "SELECT idPartida FROM partida WHERE idJugador=" + idJugador;

        var reader = con.consulta.ExecuteReader();

        List<int> partidas = new List<int>();

        while (reader.Read())
        {
            partidas.Add(reader.GetInt32(0));
        }

        reader.Close();

        // Eliminamos relaciones de acertijos
        foreach (int idPartida in partidas)
        {
            con.consulta.CommandText = "DELETE FROM partida_acertijo WHERE idPartida=" + idPartida;
            con.consulta.ExecuteNonQuery();
        }

        // Eliminamos inventarios relacionados
        con.consulta.CommandText ="DELETE FROM objeto_inventario WHERE idInventario IN (SELECT idInventario FROM partida WHERE idJugador=" + idJugador + ")";
        con.consulta.ExecuteNonQuery();

        con.consulta.CommandText ="DELETE FROM inventario WHERE idInventario IN (SELECT idInventario FROM partida WHERE idJugador=" + idJugador + ")";
        con.consulta.ExecuteNonQuery();

        // Eliminamos partidas
        con.consulta.CommandText = "DELETE FROM partida WHERE idJugador=" + idJugador;
        con.consulta.ExecuteNonQuery();
    }

    public void Volver()
    {
        SceneManager.LoadScene("MainMenu");
    }
}