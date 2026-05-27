using UnityEngine;
using System.Globalization;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;

    public Transform jugador;

    void Awake()
    {
        // Singleton para acceso global al guardado de partida
        instance = this;
    }

    public void GuardarPartida()
    {
        ConectorBD con = DBManager.instance.conexion;

        // Obtenemos la posición actual del jugador
        Vector3 pos = jugador.position;

        // Convertimos la posición a string para guardarla en BD
        string datos =
            pos.x.ToString(CultureInfo.InvariantCulture) + "," +
            pos.y.ToString(CultureInfo.InvariantCulture) + "," +
            pos.z.ToString(CultureInfo.InvariantCulture);

        // Creamos/actualizamos la partida con la posición guardada
        Partida p = new Partida();
        p.idPartida = GameManager.instance.idPartidaActual;
        p.partidaGuardada = datos;

        p.writeObject(con);

        Debug.Log("Partida guardada");
    }
}