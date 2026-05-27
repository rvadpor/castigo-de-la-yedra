using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    void Awake()
    {
        instance = this;
    }

    public Safe cajaFuerte;

    public void MarcarResuelto(int id)
    {
        if (id == 1)
        {
            cajaFuerte.AbrirCaja();
        }
    }
}