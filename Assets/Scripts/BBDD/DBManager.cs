using UnityEngine;
using CastigoYedra.BBDD;
using CastigoYedra.IO;
public class DBManager : MonoBehaviour
{
    public static DBManager instance;

    public ConectorBD conexion;
    public IOConnector sftp;

    private string nombreBD = "rvadpor116BBDD";
    private const string urlServidor = "5.45.163.43";
    private const int puertoBD = 110;
    private const string usuarioBD = "rvadpor116";
    private const string passwordBD = "rvadpor116_1";
    private const int puertoSFTP = 9000;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            InicializarConexion();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InicializarConexion()
    {
        // CONEXION MYSQL
        conexion = ConectorBD.crearConectorBD(
            ConectorBD.TIPO_MYSQL,
            nombreBD,
            urlServidor,
            puertoBD,
            usuarioBD,
            passwordBD
        );

        Debug.Log("Conexion a BD creada");

        // CONEXION SFTP
        sftp = IOConnector.Factory(
            IOConnector.TIPO_SSH_NET,
            urlServidor,
            usuarioBD,
            passwordBD,
            puertoSFTP
        );

        sftp.connect();

        Debug.Log("Conexion SFTP creada");
    }

    void OnApplicationQuit()
    {
        // CERRAR MYSQL
        if (conexion != null)
        {
            conexion.cerrar();

            Debug.Log("Conexion BD cerrada");
        }

        // CERRAR SFTP
        if (sftp != null)
        {
            sftp.close();

            Debug.Log("Conexion SFTP cerrada");
        }
    }

    public bool HayConexionBD()
    {
        try
        {
            conexion.consulta.CommandText = "SELECT 1";
            conexion.consulta.ExecuteScalar();
            return true;
        }
        catch
        {
            return false;
        }
    }
}