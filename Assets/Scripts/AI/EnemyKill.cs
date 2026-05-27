using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyKill : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    public AudioSource audioSource;
    public AudioClip sonidoMuerte;

    private void OnTriggerEnter(Collider other)
    {
        // Detecta cuando el jugador entra en el trigger del enemigo
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Muerte());
        }
    }

    IEnumerator Muerte()
    {
        // Reproduce sonido de muerte si está asignado
        if (audioSource != null && sonidoMuerte != null)
            audioSource.PlayOneShot(sonidoMuerte);

        float t = 0;
        Color color = fadeImage.color;

        // Hace un fade progresivo hasta negro (o transparente según configuración)
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = t / fadeDuration;
            fadeImage.color = color;

            yield return null;
        }

        // Cuando termina la animación, vuelve al menú principal
        SceneManager.LoadScene("MainMenu");
    }
}