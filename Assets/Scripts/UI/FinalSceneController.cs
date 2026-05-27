using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalSceneController : MonoBehaviour
{
    public AudioSource audioFinal;
    public string siguienteEscena = "Locucion";
    [Header("Sangre")]
    public Image imagenSangre;
    public float tiempoMotosierra = 5f;

    void Start()
    {
        StartCoroutine(EsperarAudio());
    }

    IEnumerator EsperarAudio()
    {
        yield return new WaitForSeconds(tiempoMotosierra);

        Color color = imagenSangre.color;
        color.a = 1f;
        imagenSangre.color = color;
        // Espera a que termine el audio
        yield return new WaitWhile(() => audioFinal.isPlaying);

        // Cambia de escena
        SceneManager.LoadScene(siguienteEscena);
    }
}