using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    public Image gameOverImage;
    public float fadeSpeed = 2f;

    void Awake()
    {
        instance = this;
        SetAlpha(0);
    }

    public void ShowGameOver()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        Color c = gameOverImage.color;

        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            c.a = t;
            gameOverImage.color = c;
            yield return null;
        }
    }

    void SetAlpha(float a)
    {
        Color c = gameOverImage.color;
        c.a = a;
        gameOverImage.color = c;
    }
}