using UnityEngine;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;

public class PickupItem : Interactable
{
    public int itemID;
    public string itemNombre;

    public override void Interact()
    {
        // Comprobamos conexión con la base de datos antes de hacer nada
        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con el servidor");
            return;
        }

        try
        {
            Debug.Log("Recogiendo objeto: " + itemNombre);

            // Creamos el item en memoria
            Item nuevoItem = new Item();
            nuevoItem.id = itemID;
            nuevoItem.nombre = itemNombre;

            // Lo añadimos al inventario del jugador
            InventoryManager.instance.AñadirItem(nuevoItem, true);

            // Guardamos el objeto en la base de datos dentro del inventario actual
            ConectorBD con = DBManager.instance.conexion;

            Inventario inv = new Inventario();
            inv.idInventario = GameManager.instance.idInventarioActual;

            inv.anadirObjeto(con, itemID);

            Debug.Log("Objeto guardado en BD");

            // Caso especial: si se recoge el item 3, se activa el enemigo
            if (itemID == 3)
            {
                EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();

                if (spawner != null)
                    spawner.ActivarEnemigo();
            }

            // Eliminamos el objeto del escenario
            Destroy(gameObject);
        }
        catch
        {
            UIMessage.instance.Mostrar("Error de conexión");
        }
    }
}