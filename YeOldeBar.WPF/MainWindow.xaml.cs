using System;
using System.Collections.Generic;
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

namespace YeOldePub.WPF 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ListBox> ListBoxes;
        
        public MainWindow()
        {
            InitializeComponent();
            ListBoxes = new List<ListBox> { lbBartender, lbPatrons, lbWaitress };
            DataManager dataManager = new DataManager(ref ListBoxes);
            //dataManager.MessageLogged += RefreshList;

        }
        


        private async void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnOpenClosePub_Click(object sender, RoutedEventArgs e)
        {
            DataManager.OpenClosePub();
        }
       
       

    }
}
