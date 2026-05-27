using UnityEngine;
using System.Collections;

public class WorldPuzzle : MonoBehaviour
{
    public int idAcertijo;
    public Safe caja;
    public GameObject puertaCaja;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if (WorldStateManager.instance.AcertijoResuelto(idAcertijo))
        {
            caja.AbrirCaja();

            if (puertaCaja != null)
                Destroy(puertaCaja);
        }
    }
}