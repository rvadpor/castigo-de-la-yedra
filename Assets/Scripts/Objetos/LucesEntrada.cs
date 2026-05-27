using UnityEngine;

public class LucesEntrada : Interactable
{
    public bool encendidas = false;
    public GameObject[] luces;

    public override void Interact()
    {
        EncenderLuces();
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
