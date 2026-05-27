using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public int idObjeto;

    void Start()
    {
        if (WorldStateManager.instance.ObjetoRecogido(idObjeto))
        {
            Destroy(gameObject);
        }
    }
}