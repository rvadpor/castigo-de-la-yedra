using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI interactionText;
    public TextMeshProUGUI pressEText;

    void Awake()
    {
        // Singleton para acceso global a la UI del juego
        instance = this;
    }

    public void MostrarMensaje(string mensaje)
    {
        // Reinicia cualquier mensaje anterior para evitar solapamientos
        StopAllCoroutines();
        StartCoroutine(MostrarMensajeTemporal(mensaje));
    }

    IEnumerator MostrarMensajeTemporal(string mensaje)
    {
        // Muestra mensaje en pantalla
        interactionText.text = mensaje;

        // Se muestra durante unos segundos
        yield return new WaitForSeconds(3f);

        // Se limpia el texto después del tiempo
        interactionText.text = "";
    }

    public void LimpiarMensaje()
    {
        // Limpia el mensaje manualmente
        interactionText.text = "";
    }

    public void MostrarPressE(bool estado)
    {
        // Activa o desactiva el aviso de interacción
        pressEText.gameObject.SetActive(estado);
    }
}