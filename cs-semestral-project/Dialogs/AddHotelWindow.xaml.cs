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
using System.Data.Entity;

namespace cs_semestral_project.Dialogs
{
    /// <summary>
    /// Logika interakcji dla klasy AddHotelWindow.xaml
    /// </summary>
    public partial class AddHotelWindow : Window, IValidable
    {
        private readonly HotelDatabaseEntities context;
        private readonly CollectionViewSource hotelSource;
        private readonly CollectionViewSource addressSource;
        private readonly hotel hotelObj;
        public AddHotelWindow(HotelDatabaseEntities context)
        {
            InitializeComponent();
            hotelSource = (CollectionViewSource)FindResource("hotelViewSource");
            addressSource = (CollectionViewSource)FindResource("addressViewSource");
            this.context = context;
        }

        /// <summary>
        /// Invoke when you want to edit hotel
        /// </summary>
        /// <param name="hotelId">id of hotel to edit</param>
        public AddHotelWindow(HotelDatabaseEntities context, hotel hotelObj) : this(context)
        {
            this.hotelObj = hotelObj;
        }

        /// <summary>
        /// Invoked when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (hotelObj != null)
            {
                hotelGrid.DataContext = hotelObj;
            }
            else
            {
                var h = new hotel();
                h.address = new address();
                hotelGrid.DataContext = h;
            }
        }

        /// <summary>
        /// Invoked when user clicks OK in dialog
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
            if (hotelObj == null)
            {
                context.hotel.Add((hotel)hotelGrid.DataContext);
            }
            context.SaveChanges();
            Close();
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
        /// Validates values for hotel
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            int nameLength = nameTextBox.Text.Length;
            int addressLength = addressInput.Text.Length;
            int postcodeLength = postCodeInput.Text.Length;
            int cityLength = cityInput.Text.Length;
            int phoneLength = phoneInput.Text.Length;
            int stars = starsDropdown.SelectedIndex;
            if (stars < 1)
            {
                MessageBox.Show("Wybierz ocenę hotelu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            bool returnValue = nameLength > 0 && nameLength < 250;
            returnValue &= addressLength > 0 && addressLength < 60;
            returnValue &= postcodeLength > 0 && postcodeLength < 7;
            returnValue &= cityLength > 0 && cityLength < 40;
            returnValue &= phoneLength > 0 && phoneLength < 12;
            return returnValue;
        }
    }
}
