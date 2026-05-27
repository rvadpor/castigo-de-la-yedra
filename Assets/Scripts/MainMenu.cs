using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject ButtonJugar;
    public GameObject ButtonSalir;
    public GameObject ButtonDificultad;

    public GameObject PanelDificultad;

    void Start()
    {
        MostrarMenuPrincipal();
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MostrarMenuPrincipal()
    {
        ButtonJugar.SetActive(true);
        ButtonSalir.SetActive(true);
        ButtonDificultad.SetActive(true);
        PanelDificultad.SetActive(false);
    }

    public void MostrarOpcionesDificultad()
    {
        ButtonJugar.SetActive(false);
        ButtonSalir.SetActive(false);
        ButtonDificultad.SetActive(false);
        PanelDificultad.SetActive(true);
    }

    public void SeleccionarDificultadFacil()
    {
        PlayerPrefs.SetString("Dificultad", "Facil");
        MostrarMenuPrincipal();
    }

    public void SeleccionarDificultadNormal()
    {
        PlayerPrefs.SetString("Dificultad", "Normal");
        MostrarMenuPrincipal();
    }

    public void SeleccionarDificultadDificil()
    {
        PlayerPrefs.SetString("Dificultad", "Dificil");
        MostrarMenuPrincipal();
    }
}
