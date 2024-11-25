using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Inventory_ProDesk
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

        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            HandlePlaceholderFocus(InputTextBox, PlaceholderLabel, "Enter your username");
            HandlePlaceholderFocus(passwordTextBox, passwordPlaceholderLabel, "Enter your password");
        }

        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HandlePlaceholderLostFocus(InputTextBox, PlaceholderLabel, "Enter your username");
            HandlePlaceholderLostFocus(passwordTextBox, passwordPlaceholderLabel, "Enter your password");
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(InputTextBox.Text))
            {
                PlaceholderLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = InputTextBox.Text;
            string password = "Enter your password";

            if (ValidateLogin(username, password))
            {
                MainForm mainForm = new MainForm();
                MainWindow window = new MainWindow();
                mainForm.Show();
                window.Close();
            }
            else
            {
                ShowError("Invalid username or password.");
            }
        }
        #region Helper

        private void HandlePlaceholderFocus(TextBox textBox, Label placeholderLabel, string defaultText)
        {
            if (textBox.Text == defaultText)
            {
                placeholderLabel.Content = defaultText;
                placeholderLabel.Visibility = Visibility.Visible;
                textBox.Text = string.Empty;
            }
        }


        private void HandlePlaceholderLostFocus(TextBox textBox, Label placeholderLabel, string defaultText)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = defaultText;
                placeholderLabel.Visibility = Visibility.Collapsed;
            }
        }


        private void ShowError(string errorMessage)
        {
            InputTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            passwordTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            ErrorPlaceholderLabel.Content = errorMessage;
        }

        private bool ValidateLogin(string username, string password)
        {
            string cnx = "Data Source=DESKTOP-4DM0DVG\\SQLEXPRESS;Initial Catalog=InventoryProDeskDB;Integrated Security=True;Trust Server Certificate=True";
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
            using(SqlConnection connection = new SqlConnection(cnx)) 
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query,connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        int result = (int)cmd.ExecuteScalar();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to database: " + ex.Message);
                    return false;
                }    
            }
        }
        #endregion
    }
}