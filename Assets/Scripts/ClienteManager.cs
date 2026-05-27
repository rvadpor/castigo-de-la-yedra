using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CastigoYedra.BBDD;
using CastigoYedra.Clases;
using CastigoYedra.IO;
public class ClienteManager : MonoBehaviour
{
    public GameObject clientePrefab;
    public Transform panelClientes;

    private string nombreBD = "rvadpor116BBDD";
    private const string urlServidor = "5.45.163.43";
    private const int puertoBD = 110;
    private const string usuarioBD = "rvadpor116";
    private const string passwordBD = "rvadpor116_1";

    private const int puertoSFTP = 9000;
    private const string usuarioSFTP = "rvadpor116";
    private const string passwordSFTP = "rvadpor116_1";

    private ConectorBD_MySQL con;
    private IOConnector sftp;

    void Start()
    {
        con = new ConectorBD_MySQL(nombreBD, urlServidor, puertoBD, usuarioBD, passwordBD);
        con.getCon();

        sftp = IOConnector.Factory(IOConnector.TIPO_SSH_NET, urlServidor, usuarioSFTP, passwordSFTP, puertoSFTP);
        sftp.connect();

        Jugador c = new Jugador();
        List<object> listaClientes = c.readObjects(con);

        // Mostrar clientes con hilo para que no reviente la ui del juego
        StartCoroutine(MostrarClientes(listaClientes));

        con.cerrar();
    }

    private IEnumerator MostrarClientes(List<object> listaClientes)
    {
        foreach (object obj in listaClientes)
        {
            Jugador cliente = (Jugador)obj;
            GameObject go = Instantiate(clientePrefab, panelClientes);

            // Asigno los campos a los textos
            Transform tNombre = go.transform.Find("NombreText");
            Transform tDni = go.transform.Find("DniText");
            //Transform tEmail = go.transform.Find("EmailText");
            //Transform tImagen = go.transform.Find("FotoImage");

            if (tNombre != null)
                tNombre.GetComponent<Text>().text = "Nombre: "+cliente.nombre;
            if (tDni != null)
                tDni.GetComponent<Text>().text = "ID: "+cliente.idJugador.ToString();
            /*if (tEmail != null)
                tEmail.GetComponent<Text>().text = cliente.email;

            if (tImagen != null && !string.IsNullOrEmpty(cliente.rutaFoto))
            {
                yield return StartCoroutine(LoadSpriteFromIOConnector(cliente.rutaFoto, tImagen.GetComponent<Image>()));
            }*/

            yield return null; //Esto lo que hace es esperar un frame para no bloquear unity, eso dice el oráculo
        }
    }

    private IEnumerator LoadSpriteFromIOConnector(string remotePath, Image targetImage)
    {
        string localPath = Application.temporaryCachePath + "/" + Path.GetFileName(remotePath);

        sftp.download(remotePath, localPath);

        // Esperar hasta que el archivo exista
        float timeout = 5f;
        float timer = 0f;
        while (!File.Exists(localPath) && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!File.Exists(localPath))
        {
            Debug.LogWarning("No se pudo descargar la imagen: " + remotePath);
            yield break;
        }

        // Cargar la textura y asignar el Sprite
        Texture2D tex = IOConnector.LoadTexture(localPath);
        if (tex != null)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            targetImage.sprite = sprite;
        }

        yield return null;
    }

    void OnDestroy()
    {
        if (sftp != null)
        {
            sftp.close();
        }
    }
    public void Salir()
    {
        SceneManager.LoadScene("MainMenu");
    }

}