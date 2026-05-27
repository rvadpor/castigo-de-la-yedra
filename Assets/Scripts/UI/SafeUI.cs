using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class SafeUI : MonoBehaviour
{
    public static SafeUI instance;

    public GameObject safePanel;
    public InputField codeInput;

    public string codigoCorrecto;

    Safe currentSafe;

    void Awake()
    {
        // Singleton para acceso global a la UI de la caja fuerte
        instance = this;
    }

    void Update()
    {
        // Permite validar el código con Enter
        if (safePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
            ComprobarCodigo();

        // Permite cerrar el panel con Escape
        if (safePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            Cerrar();
    }

    public void AbrirPanel(Safe safe)
    {
        // Pausamos el juego al abrir la caja fuerte
        Time.timeScale = 0;

        safePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        PlayerMovement.puedeMoverse = false;

        // Limpiamos input anterior
        codeInput.text = "";

        // Guardamos referencia a la caja actual
        currentSafe = safe;
        codigoCorrecto = safe.codigo;

        codeInput.ActivateInputField();
    }

    void ComprobarCodigo()
    {
        // Comprobamos conexión antes de validar y guardar estado
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            // Si el código es correcto abrimos la caja
            if (codeInput.text == codigoCorrecto)
            {
                currentSafe.AbrirCaja();

                // Marcamos el acertijo como resuelto en la partida
                PartidaAcertijo pa = new PartidaAcertijo();
                pa.idPartida = GameManager.instance.idPartidaActual;
                pa.idAcertijo = 1;
                pa.marcarResuelto(DBManager.instance.conexion);

                UIManager.instance.MostrarMensaje("Código correcto");

                Cerrar();
            }
            else
            {
                UIManager.instance.MostrarMensaje("Código incorrecto");
                Cerrar();
            }
        }
        catch
        {
            UIMessage.instance.Mostrar("Error al guardar la partida");
        }
    }

    void Cerrar()
    {
        // Reanudamos el juego y cerramos la UI
        Time.timeScale = 1;

        safePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement.puedeMoverse = true;
    }
}