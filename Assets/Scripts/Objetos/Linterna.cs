using UnityEngine;

public class Linterna : MonoBehaviour
{
    public Light luzLinterna;

    private bool encendida = false;

    public AudioSource audioSource;
    public AudioClip sonidoClick;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            encendida = !encendida;
            luzLinterna.enabled = encendida;

            if (audioSource != null && sonidoClick != null)
            {
                audioSource.PlayOneShot(sonidoClick);
            }
        }
    }
}
