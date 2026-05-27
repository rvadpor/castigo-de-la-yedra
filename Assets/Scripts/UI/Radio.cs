using UnityEngine;

public class Radio : Interactable
{
    public AudioSource audioSource;
    public AudioClip musica;
    public AudioClip sonidoClick;

    private bool encendida = false;

    public override void Interact()
    {
        CambiarEstado();
    }

    void CambiarEstado()
    {
        encendida = !encendida;

        if (encendida)
        {
            EncenderRadio();
            audioSource.PlayOneShot(sonidoClick);
        }
        else
        {
            ApagarRadio();
            audioSource.PlayOneShot(sonidoClick);
        }
    }

    void EncenderRadio()
    {
        if (audioSource != null && musica != null)
        {
            audioSource.clip = musica;
            audioSource.loop = true;
            audioSource.Play();
        }

        Debug.Log("Radio encendida");
    }

    void ApagarRadio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        Debug.Log("Radio apagada");
    }
}
