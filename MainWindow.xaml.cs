using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mvvm_ftpclient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void bCloseApp_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void bHideApp_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void bAuthorGitHub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/keyldev");
        }
    }
}
