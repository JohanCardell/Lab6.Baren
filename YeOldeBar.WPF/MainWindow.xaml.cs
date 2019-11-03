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
        List<ListBox> listBoxes;
        public MainWindow()
        {
            InitializeComponent();
            while()
            listBoxes = new List<ListBox> { lbBartender, lbWaitress, lbPatrons };
            ye
            
        }
        


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpenClosePub_Click(object sender, RoutedEventArgs e)
        {
            DataManager.OpenClosePub();
        }

    }
}
