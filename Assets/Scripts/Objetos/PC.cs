using UnityEngine.SceneManagement;

public class PC : Interactable
{
    public string itemNecesario;
    public bool abierta = false;

    public override void Interact()
    {
        if (abierta)
            return;

        if (InventoryManager.instance.TieneItem(itemNecesario))
        {
            FinalEscena();
        }
        else
        {
            UIManager.instance.MostrarMensaje("Necesitas " + itemNecesario);
        }
    }

    void FinalEscena()
    {
        abierta = true;

        SceneManager.LoadScene("Final");
    }
}
