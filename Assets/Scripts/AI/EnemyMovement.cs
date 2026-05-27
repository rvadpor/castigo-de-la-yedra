using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] destinations;

    private int i = 0;

    [Header("Follow Player")]
    public bool followPlayer;

    private GameObject player;

    public float distanceToFollowPath = 2;
    private float distanceToPlayer;

    // distancia a partir de la cual el enemigo empieza a seguir al jugador
    private float distanceToFollow = 1000000000;

    void Start()
    {
        // Buscamos el jugador por tag para poder seguirlo
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // calculamos la distancia entre enemigo y jugador cada frame
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // si el jugador está cerca y está activado el follow, lo persigue
        if (distanceToPlayer <= distanceToFollow && followPlayer)
        {
            FollowPlayer();
        }
        else
        {
            EnemyPath();
        }
    }

    public void EnemyPath()
    {
        // movimiento del enemigo siguiendo puntos de patrulla
        navMeshAgent.destination = destinations[i].position;

        // cuando llega a un punto, pasa al siguiente
        if (Vector3.Distance(transform.position, destinations[i].position) <= distanceToFollowPath)
        {
            if (destinations[i] != destinations[destinations.Length - 1])
                i++;
            else
                i = 0;
        }
    }

    public void FollowPlayer()
    {
        // el enemigo sigue directamente la posición del jugador
        navMeshAgent.destination = player.transform.position;
    }
}