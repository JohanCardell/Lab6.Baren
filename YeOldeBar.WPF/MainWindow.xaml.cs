using System;
using System.Windows;
using System.Windows.Threading;

namespace YeOldePubSim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataManager dataManager;

        public MainWindow()
        {
            InitializeComponent();
            dataManager = new DataManager(this);
        }

        private void BtnOpenPub_Click(object sender, RoutedEventArgs e)
        {
            dataManager.OpenPub();
            btnOpenPub.IsEnabled = false;
        }

    }
}
