using CastigoYedra.BBDD;
using UnityEngine;

public class ReiniciarBD : MonoBehaviour
{
    public GameObject panelConfirmacion;
    public void MostrarConfirmacion()
    {
        panelConfirmacion.SetActive(true);
    }

    // BOTON NO
    public void Cancelar()
    {
        panelConfirmacion.SetActive(false);
    }

    // BOTON SI
    public void ConfirmarReinicio()
    {
        panelConfirmacion.SetActive(false);

        ReiniciarBaseDatos();
    }
    public void ReiniciarBaseDatos()
    {
        ConectorBD con = DBManager.instance.conexion;

        if (!DBManager.instance.HayConexionBD())
        {
            UIMessage.instance.Mostrar("No hay conexión con la base de datos");
            return;
        }

        try
        {
            // DESACTIVAR FOREIGN KEYS
            con.consulta.CommandText = "SET FOREIGN_KEY_CHECKS = 0";
            con.consulta.ExecuteNonQuery();

            // BORRAR TABLAS
            con.consulta.CommandText = "DROP TABLE IF EXISTS objeto_inventario";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText = "DROP TABLE IF EXISTS partida_acertijo";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText = "DROP TABLE IF EXISTS partida";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText = "DROP TABLE IF EXISTS acertijo";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText = "DROP TABLE IF EXISTS objeto";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText = "DROP TABLE IF EXISTS inventario";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText = "DROP TABLE IF EXISTS jugador";
            con.consulta.ExecuteNonQuery();

            // ACTIVAR FOREIGN KEYS
            con.consulta.CommandText = "SET FOREIGN_KEY_CHECKS = 1";
            con.consulta.ExecuteNonQuery();

            // CREAR TABLAS

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS jugador(
                idJugador INT AUTO_INCREMENT PRIMARY KEY,
                nombre VARCHAR(50),
                imagen VARCHAR(255)
            )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS inventario(
                idInventario INT AUTO_INCREMENT PRIMARY KEY,
                tamanio INT
            )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS objeto(
                idObjeto INT AUTO_INCREMENT PRIMARY KEY,
                nombre VARCHAR(50),
                tipo VARCHAR(50)
            )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS acertijo(
                idAcertijo INT AUTO_INCREMENT PRIMARY KEY,
                nombre VARCHAR(50),
                tipo VARCHAR(50),
                descripcion VARCHAR(5000)
            )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS partida(
                idPartida INT AUTO_INCREMENT PRIMARY KEY,
                partidaGuardada VARCHAR(5000),
                idJugador INT NULL,
                idInventario INT NULL,

                FOREIGN KEY(idJugador)
                REFERENCES jugador(idJugador)
                ON DELETE SET NULL
                ON UPDATE CASCADE,

                FOREIGN KEY(idInventario)
                REFERENCES inventario(idInventario)
                ON DELETE SET NULL
                ON UPDATE CASCADE
            )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS partida_acertijo(
                idPartida INT,
                idAcertijo INT,
                resuelto BOOLEAN DEFAULT FALSE,

                PRIMARY KEY(idPartida,idAcertijo),

                FOREIGN KEY(idPartida)
                REFERENCES partida(idPartida),

                FOREIGN KEY(idAcertijo)
                REFERENCES acertijo(idAcertijo)
            )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"CREATE TABLE IF NOT EXISTS objeto_inventario(
                idObjeto INT,
                idInventario INT,

                PRIMARY KEY(idObjeto,idInventario),

                FOREIGN KEY(idObjeto)
                REFERENCES objeto(idObjeto),

                FOREIGN KEY(idInventario)
                REFERENCES inventario(idInventario)
            )";
            con.consulta.ExecuteNonQuery();

            // INSERTS
            
            con.consulta.CommandText =
            @"INSERT INTO objeto(nombre,tipo)
              VALUES('Llave','clave')";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"INSERT INTO objeto(nombre,tipo)
              VALUES('LlaveClase','acceso')";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"INSERT INTO objeto(nombre,tipo)
              VALUES('Pendrive','datos')";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"INSERT INTO acertijo(nombre,tipo,descripcion)
              VALUES(
                'Caja fuerte',
                'codigo',
                'Caja fuerte con combinación'
              )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"INSERT INTO acertijo(nombre,tipo,descripcion)
              VALUES(
                'Puerta principal',
                'llave',
                'Puerta que necesita llave'
              )";
            con.consulta.ExecuteNonQuery();

            con.consulta.CommandText =
            @"INSERT INTO acertijo(nombre,tipo,descripcion)
              VALUES(
                'Ordenador',
                'pendrive',
                'Ordenador que necesita pendrive'
              )";
            con.consulta.ExecuteNonQuery();

            UIMessage.instance.Mostrar("Base de datos reiniciada correctamente");
        }
        catch (System.Exception)
        {
            UIMessage.instance.Mostrar("Error al reiniciar la base de datos");
        }
    }
}
