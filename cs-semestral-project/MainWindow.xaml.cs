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

        /// <summary>
        /// Invoked when window loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            ReloadHotels();
        }

        /// <summary>
        /// Reloads hotels and sets first room as selected, if hotel has any
        /// </summary>
        private void ReloadHotels()
        {
            context.hotel.Load();
            hotelViewSource.Source = context.hotel.Local;
            var hotel = context.hotel.Local.First();
            if (hotel != null)
            {
                LoadRooms(hotel.hotel_id);
            }
        }

        /// <summary>
        /// Reloads rooms dropdown for hotel
        /// </summary>
        /// <param name="hotelId">selected hotel id</param>
        private void LoadRooms(int hotelId)
        {
            roomViewSource.Source = (from room in context.room where room.hotel_id == hotelId select room).ToList();
        }

        /// <summary>
        /// Invoked when hotel dropdown changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHotelChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                LoadRooms(((hotel)e.AddedItems[0]).hotel_id);
            }
        }
    }
}
