using UnityEngine;

public class ScreamerDistance : MonoBehaviour
{
    public Transform player;
    public AudioSource audioSource;
    public float distanciaActivacion = 5f;

    private bool yaActivado = false;
    private float distancia;

    void Update()
    {
        distancia = Vector3.Distance(transform.position, player.position);

        if (distancia < distanciaActivacion && !yaActivado)
        {
            audioSource.Play();
            yaActivado = true;
        }
    }
}