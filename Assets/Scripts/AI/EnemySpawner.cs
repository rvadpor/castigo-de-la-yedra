using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public void ActivarEnemigo()
    {
        enemy.SetActive(true);
        AudioManager.instance.ReproducirMusicaEnemigo();
    }
}