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
using System.Windows.Data;

namespace cs_semestral_project.Dialogs
{
    /// <summary>
    /// Logika interakcji dla klasy AddRoomWindow.xaml
    /// </summary>
    public partial class AddRoomWindow : Window, IValidable
    {
        private readonly HotelDatabaseEntities context;
        private readonly CollectionViewSource roomViewSource;
        private readonly room roomObj;
        private readonly int hotelId;
        public AddRoomWindow(HotelDatabaseEntities context, int hotelId)
        {
            InitializeComponent();
            roomViewSource = (CollectionViewSource)FindResource("roomViewSource");
            this.context = context;
            this.hotelId = hotelId;
        }
        public AddRoomWindow(HotelDatabaseEntities context, int hotelId, room editRoom) : this(context, hotelId)
        {
            roomObj = editRoom;
        }

        /// <summary>
        /// Invoked when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (roomObj != null)
            {
                roomGrid.DataContext = roomObj;
            }
            else
            {
                var r = new room() { hotel_id = hotelId };
                roomGrid.DataContext = r;
            }
        }

        /// <summary>
        /// Invoked when user clicks cancel button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Invoked when users press OK button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOKButton(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                MessageBox.Show("Wprowadzono niepoprawne dane, zweryfikuj je!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (roomObj == null)
            {
                context.room.Add((room)roomGrid.DataContext);
            }
            context.SaveChanges();
            Close();
        }

        /// <summary>
        /// Validates room data
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            int nameLength = nameTextBox.Text.Length;
            int size = sizeComboBox.SelectedIndex;
            return nameLength > 0 && nameLength < 250 && size > 0;
        }
    }
}
