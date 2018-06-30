using ClientApp.Views;
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
using System.Windows.Shapes;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ViewSalespeople_Click(object sender, RoutedEventArgs e)
        {
            SalespeopleView salespeopleView = new SalespeopleView();
            ActiveItem.Content = salespeopleView;
        }

        private void ViewDistricts_Click(object sender, RoutedEventArgs e)
        {
            DistrictsView districtsView = new DistrictsView();
            ActiveItem.Content = districtsView;
        }

        private void ViewStores_Click(object sender, RoutedEventArgs e)
        {
            StoresView storesView = new StoresView();
            ActiveItem.Content = storesView;
        }
    }
}
