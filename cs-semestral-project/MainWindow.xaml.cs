using System;
using System.Collections.Generic;
using System.Data;
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
using System.Data.Entity;
using System.Windows.Data;

namespace cs_semestral_project
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HotelDatabaseEntities context = new HotelDatabaseEntities();
        private readonly CollectionViewSource hotelViewSource;
        private readonly CollectionViewSource roomViewSource;
        public MainWindow()
        {
            InitializeComponent();
            hotelViewSource = (CollectionViewSource)FindResource("hotelViewSource");
            roomViewSource = (CollectionViewSource)FindResource("roomViewSource");
            this.DataContext = this;
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            ReloadHotels();
        }

        private void ReloadHotels()
        {
            context.hotel.Load();
            hotelViewSource.Source = context.hotel.Local;
        }
    }
}
