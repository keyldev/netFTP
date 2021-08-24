using mvvm_ftpclient.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace mvvm_ftpclient.MVVM.ViewModel
{
    class TiledFileViewModel
    {
        public RelayCommand ClickCommand { get; set; }
        public string FileURL { get; set; }
        public string Name { get; set; }
        public string FileIcon { get; set; }
        public string DateTime { get; set; }
        public string FileSize { get; set; }

        public RelayCommand DownloadClickButton { get; set; }
        public TiledFileViewModel(MainMenuViewModel main)
        {

            ClickCommand = new RelayCommand(o =>
            {
                // main.fillFiles(Name);
                main.refreshConnectiion(Name);
                MessageBox.Show(Name);
            });
            DownloadClickButton = new RelayCommand(o =>
            {
                //MessageBox.Show(Name);
                try
                {
                    main.downloadFromFile(Name);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }
    }
}
