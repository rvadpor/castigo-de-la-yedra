using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;

    public AudioClip musicaExploracion;
    public AudioClip musicaEnemigo;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ReproducirMusicaExploracion();
    }

    public void ReproducirMusicaExploracion()
    {
        musicSource.clip = musicaExploracion;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void ReproducirMusicaEnemigo()
    {
        musicSource.Stop();

        musicSource.clip = musicaEnemigo;
        musicSource.volume = 0.6f;
        musicSource.loop = true;

        musicSource.Play();
    }
}