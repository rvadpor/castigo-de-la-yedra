using TMPro;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    public static UIMessage instance;

    public GameObject panel;
    public TMP_Text texto;
    public int prueba;

    void Awake()
    {
        // Singleton para poder mostrar mensajes desde cualquier clase del juego
        instance = this;

        // Al iniciar el juego el panel de mensajes está oculto
        panel.SetActive(false);
    }

    public void Mostrar(string mensaje)
    {
        // Activa el panel y muestra el mensaje recibido
        panel.SetActive(true);
        texto.text = mensaje;
    }

    public void Ocultar()
    {
        // Oculta el panel de mensajes
        panel.SetActive(false);
    }
}