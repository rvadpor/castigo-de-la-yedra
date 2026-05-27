using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CastigoYedra.IO
{
    public abstract class IOConnector
    {
        public const string TIPO_SSH_NET = "TIPO_SSH_NET";
        public const string TIPO_SFTP_WINSCP = "TIPO_SFTP_WINSCP";

        protected string server;
        protected string user;
        protected string password;
        protected int port;
        protected string type;


        public static IOConnector Factory(string type, string server, string user, string password, int port)
        {
            IOConnector con = null;
            switch (type)
            {
                case TIPO_SSH_NET:
                    con = new IOConnectorSFTP(server, user, password, port);
                    break;
                case TIPO_SFTP_WINSCP:

                    break;
                default:
                    Debug.LogWarning("IOConnector: Tipo no soportado: " + type);
                    break;
            }
            return con;
        }


        public abstract void upload(string rutaLocal, string rutaRemota);
        public abstract void download(string rutaRemota, string rutaLocal);
        public abstract void delete(string rutaRemota);
        public abstract List<string> listContent(string rutaRemota);
        public abstract bool exists(string rutaRemota);
        public abstract double size(string rutaRemota, double scale);
        public abstract bool isDirectory(string rutaRemota);
        public abstract void connect();
        public abstract void close();


        public double fullSize(string rutaRemota, double scale)
        {
            if (isDirectory(rutaRemota))
            {
                double total = 0;
                var files = listContent(rutaRemota);
                if (files != null)
                {
                    foreach (string f in files)
                        total += fullSize(f, scale);
                }
                return total;
            }
            else
            {
                return size(rutaRemota, scale);
            }
        }

        public void masiveUpload(List<string> rutasLocales, List<string> rutasRemotas)
        {
            if (rutasLocales == null || rutasRemotas == null || rutasLocales.Count != rutasRemotas.Count)
                throw new Exception("Los arrays son nulos o tamaños diferentes");
            for (int i = 0; i < rutasLocales.Count; i++)
                upload(rutasLocales[i], rutasRemotas[i]);
        }

        public void masiveDownload(List<string> rutasRemotas, List<string> rutasLocales)
        {
            if (rutasLocales == null || rutasRemotas == null || rutasLocales.Count != rutasRemotas.Count)
                throw new Exception("Los arrays son nulos o tamaños diferentes");
            for (int i = 0; i < rutasRemotas.Count; i++)
                download(rutasRemotas[i], rutasLocales[i]);
        }


        public static Texture2D LoadTexture(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            return tex;
        }

        public string getType() => type;
    }
}