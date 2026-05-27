using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    public GameObject panelTexto;
    public AudioSource musica;

    private bool saliendo = false;

    void Start()
    {
        // Forzamos la pantalla en negro al inicio
        Color c = fadeImage.color;
        fadeImage.color = new Color(c.r, c.g, c.b, 1f);

        // Ocultamos el texto inicial
        panelTexto.SetActive(false);

        // Iniciamos la secuencia de intro
        StartCoroutine(IntroSequence());
    }

    void Update()
    {
        // Permite avanzar la intro con teclado
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            BotonContinuar();
    }

    IEnumerator IntroSequence()
    {
        // Fade de entrada (negro → visible)
        yield return StartCoroutine(Fade(1f, 0f));

        // Mostramos texto de intro
        panelTexto.SetActive(true);

        // Reproducimos música si existe
        if (musica != null)
            musica.Play();
    }

    public void BotonContinuar()
    {
        // Evita que se dispare varias veces
        if (saliendo) return;

        saliendo = true;
        StartCoroutine(SalirIntro());
    }

    IEnumerator SalirIntro()
    {
        // Fade de salida (pantalla a negro)
        yield return StartCoroutine(Fade(0f, 1f));

        // Carga de la escena principal de juego
        SceneManager.LoadScene("Juego");
    }

    IEnumerator Fade(float alphaInicial, float alphaFinal)
    {
        float tiempo = 0f;
        Color color = fadeImage.color;

        // Animación suave del alpha usando SmoothStep
        while (tiempo < fadeDuration)
        {
            tiempo += Time.deltaTime;

            float t = tiempo / fadeDuration;
            float tSuave = Mathf.SmoothStep(0f, 1f, t);

            float alpha = Mathf.Lerp(alphaInicial, alphaFinal, tSuave);

            fadeImage.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        // Asegura valor final exacto
        fadeImage.color = new Color(color.r, color.g, color.b, alphaFinal);
    }
}