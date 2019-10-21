using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Win32;

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
            HtmlTextBox.Text = await Task.Run(() => _scrapper.ConvertHtmlToClass(_scrapper.TableElements));
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

        private void LoadHtml_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
            
                ofd.Filter = "Html (*.html)|*.html";
                if (ofd.ShowDialog() == true)
                {
                    _scrapper.HtmlLocalization = ofd.FileName;
                    HtmlLocalizationTextBox.Text = ofd.FileName;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }
        private void LoadCredentials_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "JSON (*.json)|*.json";
                if (ofd.ShowDialog() == true)
                {
                    _scrapper.CredentialsLocalization = ofd.FileName;
                    CredentialsLocalizationTextBox.Text = ofd.FileName;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
        }


        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;

        }
    }
}