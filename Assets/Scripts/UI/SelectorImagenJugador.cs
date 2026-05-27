using CastigoYedra.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectorImagenJugador : MonoBehaviour
{
    public Transform contenedor;

    public GameObject botonImagenPrefab;

    public ModificarJugadorUI modificarJugadorUI;

    private string rutaImagenes = "/home/rvadpor116/imagenes/";

    void Start()
    {
        // Cargamos todas las imágenes disponibles en el servidor
        CargarImagenes();
    }

    void CargarImagenes()
    {
        // Obtenemos la lista de imágenes desde el servidor SFTP
        List<string> imagenes = DBManager.instance.sftp.listContent(rutaImagenes);

        foreach (string ruta in imagenes)
        {
            CrearBotonImagen(ruta);
        }
    }

    void CrearBotonImagen(string rutaRemota)
    {
        GameObject boton = Instantiate(botonImagenPrefab, contenedor);

        Image imagen = boton.GetComponent<Image>();

        // Descargamos y mostramos la imagen en el botón
        StartCoroutine(CargarImagen(rutaRemota, imagen));

        // Al hacer click, seleccionamos esa imagen para el jugador
        boton.GetComponent<Button>().onClick.AddListener(() =>
        {
            modificarJugadorUI.CambiarImagen(rutaRemota);
        });
    }

    IEnumerator CargarImagen(string rutaRemota, Image imagenUI)
    {
        // Ruta temporal local para descargar la imagen
        string rutaLocal =Application.temporaryCachePath +"/" +Path.GetFileName(rutaRemota);

        // Descarga desde servidor SFTP
        DBManager.instance.sftp.download(rutaRemota, rutaLocal);

        yield return null;

        // Si el archivo existe, lo convertimos en sprite para UI
        if (File.Exists(rutaLocal))
        {
            Texture2D textura = IOConnector.LoadTexture(rutaLocal);

            Sprite sprite = Sprite.Create(textura,new Rect(0, 0, textura.width, textura.height),new Vector2(0.5f, 0.5f));

            imagenUI.sprite = sprite;
        }
    }
}