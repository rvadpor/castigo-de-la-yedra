using UnityEngine;

public class PlayerInteraction : Interactable
{
    public float distanciaInteraccion = 3f;

    void Update()
    {
        // Raycast desde la cámara hacia delante para detectar objetos interactuables
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        bool mirandoInteractuable = false;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion))
        {
            Debug.Log("Raycast golpea: " + hit.collider.name);

            // Comprobamos si el objeto tiene componente PickupItem
            PickupItem pickup = hit.collider.GetComponent<PickupItem>();

            if (pickup != null)
            {
                mirandoInteractuable = true;

                // Interacción con tecla E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Recogiendo objeto");
                    pickup.Interact();
                }
            }
            else
            {
                // Si no es item recogible, comprobamos si es otro interactuable genérico
                Interactable interactuable = hit.collider.GetComponent<Interactable>();

                if (interactuable != null)
                {
                    mirandoInteractuable = true;

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("Interactuando con objeto");
                        interactuable.Interact();
                    }
                }
            }
        }

        // Mostramos en UI si el jugador puede interactuar con algo
        UIManager.instance.MostrarPressE(mirandoInteractuable);
    }
}