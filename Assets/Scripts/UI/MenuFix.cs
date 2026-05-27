using UnityEngine;

public class MenuFix : MonoBehaviour
{
    void Start()
    {
        // Restablece el tiempo del juego por si venimos de una pausa o menú
        Time.timeScale = 1f;

        // Libera el cursor para poder interactuar con los menús
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}