using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string mensaje = "";

    public virtual void Interact()
    {
        UIManager.instance.MostrarMensaje(mensaje);
    }
}
