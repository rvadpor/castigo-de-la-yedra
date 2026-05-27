using UnityEngine;

public class Safe : Interactable
{
    public string codigo = "";
    public GameObject puertaCaja;
    public GameObject llave;
    public GameObject caja;

    bool abierta = false;

    public override void Interact()
    {
        if (!abierta)
        {
            SafeUI.instance.AbrirPanel(this);
        }
    }

    public void AbrirCaja()
    {
        abierta = true;

        //UIManager.instance.MostrarMensaje("La caja fuerte se abre");

        Destroy(puertaCaja);
        //llave.GetComponent<MeshCollider>().enabled = true;
        caja.GetComponent<BoxCollider>().enabled = false;

    }
}