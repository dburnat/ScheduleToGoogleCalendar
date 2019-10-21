using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MaterialDesignThemes.Wpf;
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
        private string _htmlLocalization;
        private string _credentialsLocalization;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ReadHTML_Click(object sender, RoutedEventArgs e)
        {
            HtmlTextBox.Text = await Task.Run(() => _scrapper.ReadHtml(_htmlLocalization));
            await Task.Delay(100);
            ConvertButton.IsEnabled = true;
            StatusTextBox.Text += $"{DateTime.Now:T} Html file read\n";
            StatusTextBox.ScrollToEnd();
        }

        private async void Convert_Click(object sender, RoutedEventArgs e)
        {
            HtmlTextBox.Text = await Task.Run(() => _scrapper.ConvertHtmlToClass(_scrapper.TableElements));
            await Task.Delay(200);
            if (_credentialsLocalization == null)
            {
                ErrorLabel.Content = "Please add credentials.json and press Convert again";
            }
            else
            {
                ErrorLabel.Content = "";
                GoogleServicesButton.IsEnabled = true;
                StatusTextBox.Text += $"{DateTime.Now:T} Html file converted\n";
                StatusTextBox.ScrollToEnd();
            }

            
        }


        private async void GoogleServices_Click(object sender, RoutedEventArgs e)
        {
            _googleApi.CreateGoogleToken(_credentialsLocalization);
            await Task.Delay(200);
            StatusTextBox.Text += $"{DateTime.Now:T} Generated Google token\n";
            StatusTextBox.ScrollToEnd();
            _googleApi.CreateGoogleCalendarService();
            await Task.Delay(200);
            AddEventsButton.IsEnabled = true;
            StatusTextBox.Text += $"{DateTime.Now:T} Created Google calendar service\n";
            StatusTextBox.ScrollToEnd();
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            _googleApi.AddEventToCalendar(_scrapper.Lessons);
            StatusTextBox.Text += $"{DateTime.Now:T} Added {_scrapper.Lessons.Count} lessons to your schedule. Enjoy\n";
            StatusTextBox.ScrollToEnd();
        }

        private async void LoadHtml_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
            
                ofd.Filter = "Html (*.html)|*.html";
                if (ofd.ShowDialog() == true)
                {
                    _htmlLocalization = ofd.FileName;
                    HtmlLocalizationTextBox.Text = ofd.FileName;
                    await Task.Delay(200);
                    ReadHtmlButton.IsEnabled = true;
                    StatusTextBox.Text += $"{DateTime.Now:T} Html file loaded\n";
                    StatusTextBox.ScrollToEnd();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }

            
        }
        private async void LoadCredentials_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "JSON (*.json)|*.json";
                if (ofd.ShowDialog() == true)
                {
                    _credentialsLocalization = ofd.FileName;
                    CredentialsLocalizationTextBox.Text = ofd.FileName;
                    await Task.Delay(200);
                    StatusTextBox.Text += $"{DateTime.Now:T} Credentials file loaded\n";
                    StatusTextBox.ScrollToEnd();
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