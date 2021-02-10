using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using cs_semestral_project.Dialogs;

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
        private readonly CollectionViewSource reservationViewSource;
        public MainWindow()
        {
            InitializeComponent();
            hotelViewSource = (CollectionViewSource)FindResource("hotelViewSource");
            roomViewSource = (CollectionViewSource)FindResource("roomViewSource");
            reservationViewSource = (CollectionViewSource)FindResource("reservationViewSource");
            this.DataContext = this;
        }

        /// <summary>
        /// Invoked when window loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            LoadHotels();
        }

        /// <summary>
        /// Reloads hotels and sets first room as selected, if hotel has any
        /// </summary>
        private void LoadHotels()
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
        /// Reloads reservations dropdown for room
        /// </summary>
        /// <param name="hotelId">selected hotel id</param>
        private void LoadReservations(int roomId)
        {
            reservationViewSource.Source = (from reservation in context.reservation where reservation.room_id == roomId select reservation).ToList();
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

        /// <summary>
        /// Invoked when room dropdown changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRoomChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                LoadReservations(((room)e.AddedItems[0]).room_id);
            }
        }

        /// <summary>
        /// On user edits row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEndEditRowReservation(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit)
            {
                return;
            }
            reservation res = (reservation)e.Row.DataContext;
            res.room_id = (int)roomDropdown.SelectedValue;
            if (context.reservation.Find(res.reservation_id) == null)
            {
                context.reservation.Add(res);
            }
            context.SaveChanges();
            LoadReservations((int)roomDropdown.SelectedValue);
        }

        /// <summary>
        /// Invoked when user clicks add hotel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddHotel(object sender, RoutedEventArgs e)
        {
            AddHotelWindow window = new AddHotelWindow(context) { Owner = this };
            window.ShowDialog();
            LoadHotels();
        }

        /// <summary>
        /// Invoked when user click edit hotel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditHotel(object sender, RoutedEventArgs e)
        {
            if (!IsHotelSelected)
            {
                // TODO Message box
                return;
            }
            AddHotelWindow window = new AddHotelWindow(context, (hotel)hotelsDropdown.SelectedItem) { Owner = this };
            window.ShowDialog();
            LoadHotels();
        }

        private bool IsHotelSelected => hotelsDropdown.SelectedIndex != -1;
    }
}
