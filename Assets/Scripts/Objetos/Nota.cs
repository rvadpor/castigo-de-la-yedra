using UnityEngine;

public class Nota : Interactable
{
    [TextArea]
    public string textoNota;
    public AudioSource audioSource;
    public AudioClip sonidoLeer;

    public override void Interact()
    {
        Debug.Log("INTERACTUANDO CON NOTA");
        Debug.Log("TEXTO NOTA: " + textoNota);

        NoteUI.instance.MostrarNota(textoNota);

        if (audioSource != null && sonidoLeer != null)
        {
            audioSource.PlayOneShot(sonidoLeer);
        }
    }
}
