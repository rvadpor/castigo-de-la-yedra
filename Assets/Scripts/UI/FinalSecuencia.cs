using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalSecuencia : MonoBehaviour
{
    public Image imagenFinal;
    public AudioSource audioFinal;

    public float duracionFade = 2f;

    void Start()
    {
        // Al iniciar la escena se ejecuta la secuencia final
        StartCoroutine(SecuenciaFinal());
    }

    IEnumerator SecuenciaFinal()
    {
        // Fade de entrada (pantalla aparece poco a poco)
        yield return StartCoroutine(Fade(0, 1));

        // Espera a que termine el audio final
        yield return new WaitWhile(() => audioFinal.isPlaying);

        // Fade de salida (pantalla se desvanece)
        yield return StartCoroutine(Fade(1, 0));

        // Volvemos al menú principal
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator Fade(float inicio, float fin)
    {
        float tiempo = 0;
        Color color = imagenFinal.color;

        // Animación progresiva del alpha de la imagen
        while (tiempo < duracionFade)
        {
            float alpha = Mathf.Lerp(inicio, fin, tiempo / duracionFade);
            imagenFinal.color = new Color(color.r, color.g, color.b, alpha);

            tiempo += Time.deltaTime;
            yield return null;
        }

        // Asegura el valor final exacto del fade
        imagenFinal.color = new Color(color.r, color.g, color.b, fin);
    }
}