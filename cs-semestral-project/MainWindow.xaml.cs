using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.Entity;
using cs_semestral_project.Dialogs;
using System.Collections.Generic;

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
            var list = (from hotel in context.hotel select hotel).ToList();
            hotelViewSource.Source = list;
            if (list.Count > 0)
            {
                LoadRooms(list.First().hotel_id);
            }
            else
            {
                roomViewSource.Source = new List<room>();
                reservationViewSource.Source= new List<reservation>();
                reservationsGrid.IsReadOnly = true;
            }
        }

        /// <summary>
        /// Reloads rooms dropdown for hotel
        /// </summary>
        /// <param name="hotelId">selected hotel id</param>
        private void LoadRooms(int hotelId)
        {
            var list = (from room in context.room where room.hotel_id == hotelId select room).ToList();
            roomViewSource.Source = list;
            if(list.Count == 0)
            {
                reservationViewSource.Source = new List<reservation>();
                reservationsGrid.IsReadOnly = true;
            }
            else
            {
                reservationsGrid.IsReadOnly = false;
            }
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
                MessageBox.Show("Najpierw wybierz hotel", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AddHotelWindow window = new AddHotelWindow(context, (hotel)hotelsDropdown.SelectedItem) { Owner = this };
            window.ShowDialog();
            LoadHotels();
        }

        /// <summary>
        /// Invoked when users clicks on add room button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAddRoom(object sender, RoutedEventArgs e)
        {
            if (!IsHotelSelected)
            {
                MessageBox.Show("Najpierw wybierz hotel", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AddRoomWindow window = new AddRoomWindow(context, SelectedHotelId) { Owner = this };
            window.ShowDialog();
            LoadRooms(SelectedHotelId);
        }

        /// <summary>
        /// Invoked when user clicks on edit room button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditRoom(object sender, RoutedEventArgs e)
        {
            if (!IsRoomSelected)
            {
                MessageBox.Show("Najpierw wybierz pokój", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AddRoomWindow window = new AddRoomWindow(context, SelectedHotelId, (room)roomDropdown.SelectedItem) { Owner = this };
            window.ShowDialog();
            LoadRooms(SelectedHotelId);
        }

        /// <summary>
        /// Invoked when user clicks delete hotel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteHotel(object sender, RoutedEventArgs e)
        {
            if (!IsHotelSelected)
            {
                MessageBox.Show("Najpierw wybierz hotel", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Czy na pewno chcesz usunąć ten hotel?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;
            context.hotel.Remove((hotel)hotelsDropdown.SelectedItem);
            context.SaveChanges();
            LoadHotels();
        }

        /// <summary>
        /// Invoked when user clicks delete room button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteRoom(object sender, RoutedEventArgs e)
        {
            if (!IsRoomSelected)
            {
                MessageBox.Show("Najpierw wybierz pokój", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Czy na pewno chcesz usunąć ten pokój?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;
            context.room.Remove((room)roomDropdown.SelectedItem);
            context.SaveChanges();
            LoadRooms(SelectedHotelId);
        }

        private bool IsHotelSelected => hotelsDropdown.SelectedIndex != -1;
        private bool IsRoomSelected => roomDropdown.SelectedIndex != -1;
        private int SelectedHotelId => ((hotel)hotelsDropdown.SelectedItem).hotel_id;
    }
}
