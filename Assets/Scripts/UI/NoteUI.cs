using UnityEngine;
using TMPro;

public class NoteUI : MonoBehaviour
{
    public static NoteUI instance;

    public GameObject notePanel;
    public TextMeshProUGUI noteText;

    bool notaAbierta = false;

    void Awake()
    {
        // Patrón singleton para poder acceder a esta clase desde cualquier sitio
        instance = this;
    }

    void Update()
    {
        // Si hay una nota abierta, se puede cerrar con la tecla Q
        if (notaAbierta && Input.GetKeyDown(KeyCode.Q))
        {
            CerrarNota();
        }
    }

    public void MostrarNota(string texto)
    {
        Debug.Log("ABRIENDO NOTA");

        // Pausamos el juego
        Time.timeScale = 0;

        // Activamos panel de la nota
        notePanel.SetActive(true);

        // Asignamos el texto de la nota
        noteText.text = texto;

        notaAbierta = true;

        // Liberamos el cursor para poder leer/interactuar con UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void CerrarNota()
    {
        // Reanudamos el juego
        Time.timeScale = 1;

        // Ocultamos panel
        notePanel.SetActive(false);

        notaAbierta = false;

        // Bloqueamos el cursor otra vez para el gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}