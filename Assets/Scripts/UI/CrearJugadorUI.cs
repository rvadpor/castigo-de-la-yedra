using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class CrearJugadorUI : MonoBehaviour
{
    public InputField inputNombre;

    ConectorBD con;

    void Start()
    {
        con = DBManager.instance.conexion;
    }

    public void CrearJugador()
    {

        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            string nombre = inputNombre.text;

            // NOMBRE VACÍO
            if (nombre == "")
            {
                UIMessage.instance.Mostrar("Introduce un nombre");
                return;
            }

            // COMPROBAR SI YA EXISTE
            con.consulta.Parameters.Clear();

            con.consulta.CommandText = "SELECT COUNT(*) FROM jugador WHERE nombre=@nombre";

            con.AddParameterWithValue("@nombre", nombre);

            int cantidad = System.Convert.ToInt32(con.consulta.ExecuteScalar());

            con.consulta.Parameters.Clear();

            if (cantidad > 0)
            {
                UIMessage.instance.Mostrar("Ese jugador ya existe");
                return;
            }

            // CREAR JUGADOR
            Jugador j = new Jugador();
            j.nombre = nombre;

            j.writeNewObject(con);

            GameManager.instance.idJugadorActual = j.idJugador;
            GameManager.instance.nombreJugador = j.nombre;

            UIMessage.instance.Mostrar("Jugador "+j.nombre+" creado correctamente");

            inputNombre.text = "";

        }
        catch
        {
            UIMessage.instance.Mostrar("Error al crear jugador");
        }
        
    }

    public void Salir()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
