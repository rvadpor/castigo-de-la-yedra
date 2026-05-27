using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalUI : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("SeleccionarJugador");
    }

    public void CrearJugador()
    {
        SceneManager.LoadScene("CrearJugador");
    }

    public void ModificarJugador()
    {
        SceneManager.LoadScene("ModificarJugador");
    }

    public void BorrarJugador()
    {
        SceneManager.LoadScene("BorrarJugador");
    }

    public void Listar()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Controles()
    {
        SceneManager.LoadScene("Controles");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }
}
