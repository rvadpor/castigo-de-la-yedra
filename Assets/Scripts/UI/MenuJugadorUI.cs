using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class MenuJugadorUI : MonoBehaviour
{
    public TMP_Text nombreJugadorText;

    void Start()
    {
        // Mostramos el nombre del jugador actual en pantalla
        nombreJugadorText.text = "Jugador: " + GameManager.instance.nombreJugador;
    }

    public void NuevaPartida()
    {
        // Comprobamos conexión con la base de datos antes de crear partida
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            ConectorBD con = DBManager.instance.conexion;

            // Creamos inventario nuevo para la partida
            Inventario inv = new Inventario();
            inv.tamanio = 10;
            inv.writeNewObject(con);

            // Creamos nueva partida asociada al jugador actual
            Partida p = new Partida();
            p.idJugador = GameManager.instance.idJugadorActual;
            p.idInventario = inv.idInventario;
            p.partidaGuardada = "";
            p.writeNewObject(con);

            // Guardamos IDs en GameManager para usarlos en otras escenas
            GameManager.instance.idInventarioActual = inv.idInventario;
            GameManager.instance.idPartidaActual = p.idPartida;

            DbCommand consulta = con.consulta;
            consulta.Parameters.Clear();

            // Insertamos todos los acertijos en estado inicial (no resueltos)
            consulta.CommandText ="INSERT INTO partida_acertijo(idPartida,idAcertijo) SELECT @idPartida,idAcertijo FROM acertijo";

            con.AddParameterWithValue("@idPartida", p.idPartida);
            consulta.ExecuteNonQuery();

            // Entramos a la cinemática de inicio
            SceneManager.LoadScene("Intro");
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al crear partida");
        }
    }

    public void CargarPartida()
    {
        // Abre la pantalla de selección de partidas guardadas
        SceneManager.LoadScene("CargarPartida");
    }

    public void Volver()
    {
        // Vuelve al menú principal
        SceneManager.LoadScene("MainMenu");
    }
}