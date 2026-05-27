using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public TextMeshProUGUI inventoryText;

    void Start()
    {
        ActualizarInventario();
    }

    void Awake()
    {
        instance = this;
    }

    public void ActualizarInventario()
    {
        inventoryText.text = "Inventario:\n";

        foreach (Item item in InventoryManager.instance.inventario)
        {
            inventoryText.text += "- " + item.nombre + "\n";
        }
    }
}