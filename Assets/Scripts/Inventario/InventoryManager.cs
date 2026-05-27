using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Instancia global del inventario (patrón Singleton)
    public static InventoryManager instance;

    // Lista donde se guardan todos los items del jugador
    public List<Item> inventario = new List<Item>();

    void Awake()
    {
        // Se asegura de que solo exista una instancia del inventario en la escena
        instance = this;
    }

    public void AñadirItem(Item item, bool mostrarMensaje = true)
    {
        // Añade el item a la lista del inventario
        inventario.Add(item);

        Debug.Log("Item añadido: " + item.nombre);

        // Muestra mensaje en pantalla si está activado
        if (mostrarMensaje)
        {
            UIManager.instance.MostrarMensaje("Has recogido: " + item.nombre);
        }

        // Actualiza la UI del inventario
        InventoryUI.instance.ActualizarInventario();
    }

    public bool TieneItem(string nombre)
    {
        // Recorre el inventario para comprobar si existe un item con ese nombre
        foreach (Item item in inventario)
        {
            if (item.nombre == nombre)
                return true;
        }

        return false;
    }
}