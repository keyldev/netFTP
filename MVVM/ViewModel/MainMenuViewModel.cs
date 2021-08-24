using mvvm_ftpclient.Core;
using mvvm_ftpclient.MVVM.Model;
using mvvm_ftpclient.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace mvvm_ftpclient.MVVM.ViewModel
{
    class MainMenuViewModel : ObservableObject
    {
        private FTPModel ftpModel = new FTPModel();
        //Window, with random tech information.
        InfoWindow info;

        #region PROPERTIES
        private string _serverAddress; // ip
        private string _serverPort; // port
        private string _serverLogin; // login
        private string _serverPassword; // pass
        private string _browserURL; // верхняя адресная строка

        public string ServerAddress
        {
            get { return _serverAddress; }
            set { _serverAddress = value; NotifyPropertyChanged(); }
        }

        public string ServerPort
        {
            get { return _serverPort; }
            set { _serverPort = value; }
        }

        public string ServerLogin
        {
            get { return _serverLogin; }
            set { _serverLogin = value; NotifyPropertyChanged(); }
        }

        public string ServerPassword
        {
            get { return _serverPassword; }
            set { _serverPassword = value; NotifyPropertyChanged(); }
        }

        public string BrowserUrl
        {
            get { return _browserURL; }
            set { _browserURL = value; NotifyPropertyChanged(); }
        }


        #endregion

        #region MVVM_Properties
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; }
        }
        public RelayCommand B_Connect { get; set; }
        public RelayCommand DownloadClick { get; set; }
        public RelayCommand GoBackClick { get; set; }
        public RelayCommand RefreshConnectionButton { get; set; }
        public RelayCommand DeleteFileClick { get; set; }
        public ObservableCollection<TiledFileViewModel> TiledFiles { get; set; }
        #endregion
        string[] files_array;
        public RelayCommand UploadFileClick { get; set; }

        Regex regex = new Regex(@"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                   RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        public MainMenuViewModel()
        {

            TiledFiles = new ObservableCollection<TiledFileViewModel>();
            B_Connect = new RelayCommand(o =>
            {
                BrowserUrl = "ftp://" + ServerAddress + ":" + ServerPort;
                ftpModel.HostIP = _serverAddress;
                ftpModel.HostPort = _serverPort;
                ftpModel.HostLogin = _serverLogin;
                ftpModel.HostPassword = _serverPassword;
                try
                {
                    files_array = ftpModel.listDirectoryDetails();
                    fillFiles();
                }
                catch (Exception ex)
                {
                    info = new InfoWindow($"Ошибка подключения\n{ex.Message}");
                    info.Show();
                }
            });
            DownloadClick = new RelayCommand(o =>  // скачать файл
            {
                //ftpModel.downloadFile("/bluetooth/a.drawio", AppDomain.CurrentDomain.BaseDirectory);
            });
            GoBackClick = new RelayCommand(o =>
            {  // вернуться назад
                refreshConnectiion();
            });
            DeleteFileClick = new RelayCommand(o =>
            {
                // ftpModel.deleteFile("/bluetooth/", "a.drawio");
            });

            RefreshConnectionButton = new RelayCommand(o =>
            {
                refreshConnectiion();
            });
            string[] files = new string[1] { @"C:\Users\skeych\Desktop\a.drawio" };
            UploadFileClick = new RelayCommand(o => {
                //ftpModel.uploadFile(files[0], "/bluetooth/");

            });
        }
        protected void fillFiles(string name = null)
        {
            string path_to_icons = "/mvvm_ftpclient;component/Assets/";
            string type = "";
            Match match;
            TiledFiles.Clear();
            for (int i = 0; i < files_array.Length; i++) // drop this code in FTPModel
            {
                match = regex.Match(files_array[i]);
                if (match.Length > 5)
                {
                    if (match.Groups[1].Value == "d")
                        type = path_to_icons + "folder.png";
                    else
                    {
                        if (match.Groups[6].Value.Contains(".png"))
                            type = path_to_icons + "FilesIcons/png.png";
                        else if (match.Groups[6].Value.Contains(".jpg"))
                            type = path_to_icons + "FilesIcons/jpg.png";
                        else if (match.Groups[6].Value.Contains(".mp4"))
                            type = path_to_icons + "FilesIcons/mpg.png";
                        else if (match.Groups[6].Value.Contains(".mov"))
                            type = path_to_icons + "FilesIcons/mov.png";
                        else if (match.Groups[6].Value.Contains(".3gp"))
                            type = path_to_icons + "FilesIcons/mpg.png";
                        else if (match.Groups[6].Value.Contains(".pdf"))
                            type = path_to_icons + "FilesIcons/pdf.png";
                        else if (match.Groups[6].Value.Contains(".xlsx"))
                            type = path_to_icons + "FilesIcons/xls.png";
                        else if (match.Groups[6].Value.Contains(".docx") || match.Groups[6].Value.Contains(".doc"))
                            type = path_to_icons + "FilesIcons/doc.png";
                        else if (match.Groups[6].Value.Contains(".mp3"))
                            type = path_to_icons + "FilesIcons/mp3.png";
                        else if (match.Groups[6].Value.Contains(".html"))
                            type = path_to_icons + "FilesIcons/html.png";
                        else if (match.Groups[6].Value.Contains(".php"))
                            type = path_to_icons + "FilesIcons/php.png";
                        else
                            type = path_to_icons + "FilesIcons/zip.png";
                    }
                    string filesize = (Int32.Parse(match.Groups[3].Value.Trim()) / 1024).ToString();
                    string temp_name = match.Groups[6].Value;
                    TiledFiles.Add(new TiledFileViewModel(this)
                    {
                        Name = temp_name,
                        FileIcon = type,
                        DateTime = "Date: " + match.Groups[4].Value,
                        FileSize = "Size: " + filesize + " kB"
                    });
                }
            }
        }
        //string prev_temp_name = null;
        #region PUBLIC_METHODS
        public void refreshConnectiion(string name = null) // здесь баг, нужно передавать текущий адрес, т.е. prev_name + name
        {
            BrowserUrl = "ftp://" + ServerAddress + ":" + ServerPort;
            ftpModel.HostIP = _serverAddress;
            ftpModel.HostPort = _serverPort;
            ftpModel.HostPort += "/" + name;
            MessageBox.Show(ftpModel.HostPort);
            ftpModel.HostLogin = _serverLogin;
            ftpModel.HostPassword = _serverPassword;
            files_array = ftpModel.listDirectoryDetails();
            fillFiles();
        }
        public void downloadFromFile(string name)
        {
            
            ftpModel.downloadFile(name, AppDomain.CurrentDomain.BaseDirectory + $"/{name}");
        }
        #endregion
    }
}
