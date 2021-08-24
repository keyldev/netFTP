using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace mvvm_ftpclient.MVVM.Model
{
    class FTPModel : INotifyPropertyChanged
    {
        #region Variables

        private bool _usePassive = true; // при значении true устанавливает пассивный режим запроса к серверу 
        private bool _useBinary = true; // Значение true указывает, что передаваемые данные являются двоичными, а значение false - что данные будут представлять текст. Значение по умолчанию — true
        private bool _enableSSL = false; // указывает, надо ли использовать ssl-соединение

        private int _buffer = 1024; // размер массива байт

        private string uri; // адрес FTP сервера загоняется сюда

        #endregion

        #region Properties
        private string _hostIP;
        private string _hostPort;
        private string _hostLogin;
        private string _hostPassword;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        public string HostIP
        {
            get { return _hostIP; }
            set { _hostIP = value; NotifyPropertyChanged(); }
        }
        public string HostPort
        {
            get { return _hostPort; }
            set { _hostPort = value; NotifyPropertyChanged(); }
        }
        public string HostLogin
        {
            get { return _hostLogin; }
            set { _hostLogin = value; }
        }
        public string HostPassword
        {
            get { return _hostPassword; }
            set { _hostPassword = value; }
        }
        #endregion

        #region Constructors
        public FTPModel()
        {

        }
        #endregion

        #region Methods
        private FtpWebRequest _createRequest(string method)
        {

            uri = "ftp://" + _hostIP + ":" + _hostPort; // URL
            //MessageBox.Show(uri);
            var request = (FtpWebRequest)WebRequest.Create(uri); // делаем веб запрос по URL

            
            if (_hostLogin == "" && _hostPassword == "")
                request.Credentials = new NetworkCredential("", "");
            else
                request.Credentials = new NetworkCredential(_hostLogin, _hostPassword);
            request.Method = method;
            request.UseBinary = _useBinary;
            request.UsePassive = _usePassive;
            request.EnableSsl = _enableSSL;

            return request;
        }
        public string getStatus(FtpWebRequest request)
        {
            using (var response = (FtpWebResponse)request.GetResponse())
            {

                return response.StatusDescription;
            }
        }
        public string[] listAllDirectories()
        {
            var list = new List<string>();
            var request = _createRequest(WebRequestMethods.Ftp.ListDirectory);

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                            list.Add(reader.ReadLine());
                    }
                }
            }
            return list.ToArray();
        }
        // не загружает больлше 1 файла за раз, исправить
        public void uploadFile(string filename, string destination)
        {
            try
            {
                FileInfo file;
                FileStream fs = null;
                Stream str = null;
                //for (int i = 0; i < filename.Length; i++)
                //{
                file = new FileInfo(filename);
                //MessageBox.Show(uri + destination + "/" + file.Name);

                HostPort += destination + "/" + file.Name;
                var request = _createRequest(WebRequestMethods.Ftp.UploadFile);

                request.ContentLength = file.Length;
                request.KeepAlive = false;
                byte[] buffLength = new byte[_buffer];
                int contentLength;

                fs = file.OpenRead();
                str = request.GetRequestStream();

                contentLength = fs.Read(buffLength, 0, _buffer);
                while (contentLength != 0)
                {
                    str.Write(buffLength, 0, contentLength);
                    contentLength = fs.Read(buffLength, 0, _buffer);
                }
                // }
                str.Close();
                fs.Close();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void deleteFile(string destination, string filename)
        {
            try
            {
                HostPort += destination + "/" + filename;

                var request = _createRequest(WebRequestMethods.Ftp.DeleteFile);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public string downloadFile(string sourceFolder, string destination)
        {
            HostPort += "/" + sourceFolder;
           
            var request = _createRequest(WebRequestMethods.Ftp.DownloadFile);

            byte[] buffer = new byte[_buffer];
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var fileStream = new FileStream(destination, FileMode.OpenOrCreate))
                    {
                        int readCount = stream.Read(buffer, 0, _buffer);
                        while (readCount > 0)
                        {
                            fileStream.Write(buffer, 0, _buffer);
                            readCount = stream.Read(buffer, 0, _buffer);
                        }
                    }
                }
                return response.StatusDescription;
            }
        }
        public string[] listDirectoryDetails()
        {
            var list = new List<string>();
            var request = _createRequest(WebRequestMethods.Ftp.ListDirectoryDetails);
            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, true))
                    {
                        while (!reader.EndOfStream)
                        {
                            list.Add(reader.ReadLine());
                        }
                    }
                }
            }
            return list.ToArray();
        }


        #endregion
    }
}
