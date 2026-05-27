using UnityEngine;

public class SonidoImpacto : MonoBehaviour
{
    public AudioSource sonido;

    private void OnCollisionEnter(Collision collision)
    {
        sonido.Play();
    }
}
