using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace CastigoYedra.IO
{
    public class IOConnectorSFTP : IOConnector
    {
        SftpClient sftp;

        public IOConnectorSFTP(string server, string user, string password, int port)
        {
            this.server = server;
            this.user = user;
            this.password = password;
            this.port = port;
            this.type = IOConnector.TIPO_SSH_NET;
        }

        public override void connect()
        {
            sftp = new SftpClient(server, port, user, password);
            sftp.Connect();
        }

        public override void close()
        {
            if (sftp != null && sftp.IsConnected)
            {
                sftp.Disconnect();
                sftp.Dispose();
            }
        }

        public override void upload(string rutaLocal, string rutaRemota)
        {
            using (var fs = new FileStream(rutaLocal, FileMode.Open, FileAccess.Read))
            {
                sftp.UploadFile(fs, rutaRemota);
            }
        }

        public override void download(string rutaRemota, string rutaLocal)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(rutaLocal));
            using (var fs = new FileStream(rutaLocal, FileMode.Create, FileAccess.Write))
            {
                sftp.DownloadFile(rutaRemota, fs);
            }
        }

        public override void delete(string rutaRemota)
        {
            sftp.DeleteFile(rutaRemota);
        }

        public override bool exists(string rutaRemota)
        {
            return sftp.Exists(rutaRemota);
        }

        public override bool isDirectory(string rutaRemota)
        {
            return sftp.GetAttributes(rutaRemota).IsDirectory;
        }

        public override List<string> listContent(string rutaRemota)
        {
            List<string> archivos = new List<string>();

            foreach (var file in sftp.ListDirectory(rutaRemota))
            {
                // IGNORAR . y ..
                if (file.Name != "." && file.Name != "..")
                {
                    archivos.Add(file.FullName);
                }
            }

            return archivos;
        }

        public override double size(string rutaRemota, double scale)
        {
            if (isDirectory(rutaRemota)) return 0.0;

            return sftp.GetAttributes(rutaRemota).Size;
        }
    }
}