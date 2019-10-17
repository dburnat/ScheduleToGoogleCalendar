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

namespace ScheduleToGCalendar
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Scrapper _scrapper = new Scrapper();
        readonly GoogleApi _googleApi = new GoogleApi();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ReadHTML_Click(object sender, RoutedEventArgs e)
        {
            HtmlTextBox.Text = await Task.Run(() => _scrapper.ReadHtml());
            await Task.Delay(100);
            ConvertButton.IsEnabled = true;
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {
            ConvertTextBox.Text = await Task.Run(() => _scrapper.ConvertHtmlToClass(_scrapper.TableElements));
        }


        private void GetToken_Click(object sender, RoutedEventArgs e)
        {
            _googleApi.CreateGoogleToken();
        }

        private void GetService_Click(object sender, RoutedEventArgs e)
        {
            _googleApi.CreateGoogleCalendarService();
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            _googleApi.AddEventToCalendar(_scrapper.Lessons);
        }
    }
}