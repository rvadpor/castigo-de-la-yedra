using UnityEngine;

public class LucesClase : Interactable
{
    public bool encendidas = false;
    public GameObject[] luces;
    public AudioSource audioSource;
    public AudioClip sonidoClick;

    public override void Interact()
    {
        EncenderLuces();

        if (audioSource != null && sonidoClick != null)
        {
            audioSource.PlayOneShot(sonidoClick);
        }
    }

    public void EncenderLuces()
    {
        encendidas = !encendidas;

        foreach (GameObject luz in luces)
        {
            luz.SetActive(encendidas);
        }
    }
}
