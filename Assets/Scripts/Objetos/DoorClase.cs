using UnityEngine;

public class DoorClase : Interactable
{
    public string itemNecesario;
    public bool abierta = false;
    public AudioSource audioSource;
    public AudioClip sonidoAbrir;

    public override void Interact()
    {
        if (abierta)
            return;

        if (InventoryManager.instance.TieneItem(itemNecesario))
        {
            AbrirPuerta();

            if (audioSource != null && sonidoAbrir != null)
            {
                audioSource.PlayOneShot(sonidoAbrir);
            }
        }
        else
        {
            UIManager.instance.MostrarMensaje("Necesitas " + itemNecesario);
        }
    }

    void AbrirPuerta()
    {
        abierta = true;

        //UIManager.instance.MostrarMensaje("La puerta se abre");

        transform.Rotate(0, 0, 90);
        //transform.position = new Vector3(-2.32f, 9.733f, -39.878f);
    }
}