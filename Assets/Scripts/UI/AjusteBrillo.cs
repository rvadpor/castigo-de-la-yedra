using UnityEngine;
using UnityEngine.UI;

public class AjusteBrillo : MonoBehaviour
{
    public Image panelBrillo;
    public Slider sliderBrillo;

    void Start()
    {
        // Cargar brillo guardado
        float brilloGuardado = PlayerPrefs.GetFloat("Brillo", 0.3f);

        sliderBrillo.value = brilloGuardado;

        CambiarBrillo(brilloGuardado);

        // Cambios del slider
        sliderBrillo.onValueChanged.AddListener(CambiarBrillo);
    }

    public void CambiarBrillo(float valor)
    {
        Color color = panelBrillo.color;

        color.a = 1f - valor;

        panelBrillo.color = color;

        // Guardar configuración
        PlayerPrefs.SetFloat("Brillo", valor);
        PlayerPrefs.Save();
    }
}
