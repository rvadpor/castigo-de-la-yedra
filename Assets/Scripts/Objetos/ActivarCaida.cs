using UnityEngine;

public class ActivarCaida : MonoBehaviour
{
    public Rigidbody objetoQueCae;
    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activado)
        {
            objetoQueCae.isKinematic = false;
            activado = true;
        }
    }
}