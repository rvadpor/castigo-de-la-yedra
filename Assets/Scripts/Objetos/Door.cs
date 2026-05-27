using UnityEngine;

public class Door : Interactable
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

        transform.Rotate(0, 90, 0);
        transform.position = new Vector3(33.18f, 0.5143369f, -22.06619f);
    }
}