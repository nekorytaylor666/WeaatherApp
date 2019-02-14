using System;
using System.Collections.Generic;
using System.IO;
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
using APIXULib;

namespace WeatherApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<City> cities;
        List<Forecastday> forecast;
        public MainWindow()
        {
            InitializeComponent();
            using (FileStream stream = new FileStream("usaCities.json", FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(json);
            }
        }

        private void addItem(string text)
        {
            TextBlock block = new TextBlock();

            // Add the text   
            block.Text = text;

            // A little style...   
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;

            // Mouse events   


            block.MouseLeftButtonUp += (sender, e) =>
            {
                CityInput.Text = (sender as TextBlock).Text;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.WhiteSmoke;
                b.Foreground = Brushes.Black;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
                b.Foreground = Brushes.White;

            };

            // Add to the panel   
            resultStack.Children.Add(block);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
          
        }

        void InputToComponentCityNameDateAndTemperatur()
        {
            Temperature.Text = forecast[0].day.avgtemp_c.ToString()+ "℃";
            Temperature2.Text = forecast[1].day.avgtemp_c.ToString()+ "℃";
            Temperature3.Text = forecast[2].day.avgtemp_c.ToString() + "℃";
            Temperature4.Text = forecast[3].day.avgtemp_c.ToString() + "℃";
            Temperature5.Text = forecast[4].day.avgtemp_c.ToString() + "℃";
            Temperature6.Text = forecast[5].day.avgtemp_c.ToString() + "℃";
            Temperature7.Text = forecast[6].day.avgtemp_c.ToString() + "℃";
            City.Text = CityInput.Text;
            
            Time.Text = DateTime.Today.ToShortDateString();
            Time2.Text = DateTime.Today.AddDays(1).ToShortDateString();
            Time3.Text = DateTime.Today.AddDays(2).ToShortDateString();
            Time4.Text = DateTime.Today.AddDays(3).ToShortDateString();
            Time5.Text = DateTime.Today.AddDays(4).ToShortDateString();
            Time6.Text = DateTime.Today.AddDays(5).ToShortDateString();
            Time7.Text = DateTime.Today.AddDays(6).ToShortDateString();

        }

        private void GridKeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            var data = cities;

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear   
                resultStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list   
            resultStack.Children.Clear();

            // Add the result   
            foreach (var obj in data)
            {
                if (obj.CityName.ToLower().StartsWith(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work   

                    addItem(obj.CityName);
                    found = true;
                }
            }

            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                APIXUWeatherRepository weather = new APIXUWeatherRepository();
                City.Text = CityInput.Text;
                forecast = weather.GetWeatherData("91dd720e6e5c43d9843143858191302", GetBy.CityName, CityInput.Text, Days.Seven).forecast.forecastday.ToList();
                InputToComponentCityNameDateAndTemperatur();
            }
        }
    }
}
