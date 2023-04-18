using MySql.Data.MySqlClient;
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

namespace KekhegySzallo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<room> AllData;
        private readonly string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=kekhegy;";
        private MySqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AllData=new List<room>();
            connection=new MySqlConnection(connectionString);
            connection.Open();
            string queryText = "SELECT * FROM szobak";
            MySqlCommand query=new MySqlCommand(queryText, connection);
            MySqlDataReader sqlReader=query.ExecuteReader();
            while (sqlReader.Read())
            {
                AllData.Add(new room(sqlReader));
            }
            sqlReader.Close();
            ListOutOnMenu.ItemsSource = AllData;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection.Close();
        }

        private void BookedButton_Click(object sender, RoutedEventArgs e)
        {
            if (ListOutOnMenu.SelectedIndex == -1)
                MessageBox.Show("Nincs kiválasztott szoba!");
            else
            {
                int index = ListOutOnMenu.SelectedIndex;
                int id = AllData[index].Id;
                string queryText = $"SELECT COUNT(*) FROM foglalasok INNER JOIN szobak ON foglalasok.szobaId=szobak.id WHERE szobaId={id}";
                MySqlCommand query=new MySqlCommand( queryText, connection);
                MySqlDataReader sqlReader= query.ExecuteReader();
                sqlReader.Read();
                int db = sqlReader.GetInt32(0);
                feladat4.Content = $"A kijelölt szobát {db} alkalommal foglalták";
                sqlReader.Close();
            }
        }

        private void SummBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListOutOnMenu.SelectedIndex == -1)
                MessageBox.Show("Nincs kiválasztott szoba!");
            else
            {
                int index = ListOutOnMenu.SelectedIndex;
                int id = AllData[index].Id;
                string queryText = $"SELECT SUM(ar*napok) FROM foglalasok INNER JOIN szobak ON foglalasok.szobaId=szobak.id WHERE szobaId={id}";
                MySqlCommand query = new MySqlCommand(queryText, connection);
                MySqlDataReader sqlReader = query.ExecuteReader();
                sqlReader.Read();
                int summa = sqlReader.GetInt32(0);
                feladat5.Content = $"A szoba bevétele: {summa} Ft.";
                sqlReader.Close();
            }
        }

        private void ReservationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListOutOnMenu.SelectedIndex == -1)
                MessageBox.Show("Nincs kiválasztott szoba!");
            else
            {
                int index = ListOutOnMenu.SelectedIndex;
                int id = AllData[index].Id;
                string queryText = $"SELECT DISTINCT nev FROM foglalasok INNER JOIN szobak ON foglalasok.szobaId=szobak.id INNER JOIN vendegek ON foglalasok.vendegId=vendegek.id WHERE szobaId={id}";
                MySqlCommand query = new MySqlCommand(queryText, connection);
                MySqlDataReader sqlReader = query.ExecuteReader();
                ReserverdGuests.Items.Clear();
                while (sqlReader.Read())
                {
                    ReserverdGuests.Items.Add(sqlReader.GetString(0));
                }
                sqlReader.Close();
            }
        }
    }
}
