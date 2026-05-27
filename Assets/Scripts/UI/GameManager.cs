using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int idJugadorActual;
    public string nombreJugador;
    public int idPartidaActual;
    public int idInventarioActual;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
