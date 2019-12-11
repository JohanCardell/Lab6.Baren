using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Diagnostics;

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
            
            // lbBartender.i
            //PubCountdown.Content = new System.Diagnostics.Stopwatch();
          
        }
        


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpenClosePub_Click(object sender, RoutedEventArgs e)
        {
            dataManager.OpenClosePub();
        }

        private void btnOnOffWaitress_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
