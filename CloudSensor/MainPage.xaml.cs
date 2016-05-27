using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace CloudSensor
{
    public sealed partial class MainPage : Page
    {
        //Set this value so you don't have to type it in a W10 IoT Core UI.
        private const string hardCodedDeviceConnectionString = "";

        private DispatcherTimer timer;
        private DispatcherTimer ioTimer;
        private bool backOffUp;
        private bool backOffDown;
        private bool send;
        private Random randomizer = new Random();
        private List<TimeValueItem> eventCache = new List<TimeValueItem>();
        ApplicationDataContainer localSettings = null;


        private CloudSensorHelper cs;
        public MainPage()
        {
            send = false;
            this.InitializeComponent();
            localSettings = ApplicationData.Current.LocalSettings;

            cs = new CloudSensorHelper();

            //SNIP4


            Setup();
        }

        private async void Setup()
        {

            //A timer to check sensor values
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(2000);
            this.timer.Tick += this.OnTick;
            this.timer.Start();

            //A timer to check button presses and handle those events
            this.ioTimer = new DispatcherTimer();
            this.ioTimer.Interval = TimeSpan.FromMilliseconds(50);
            this.ioTimer.Tick += IoTimer_Tick;
            this.ioTimer.Start();
        }

        private void IoTimer_Tick(object sender, object e)
        {
            //SNIP5

        }

        private void OnTick(object sender, object e)
        {
            //Backoff the timer
            timer.Stop();


            SenseAndSubmit();


            timer.Start();
        }

        private async Task SenseAndSubmit()
        {
            //Update sensor values
            cs.Sense();

            TempOffset.Text = cs.TempOffset.ToString();

            tempGauge.Value = cs.Temperature;
            lumenGauge.Value = cs.Lumen;


            if (send)
            {



                //SNIP6

                eventCache.Add(new TimeValueItem { Time = DateTime.Now.ToString("H:mm:ss"), Value = cs.Temperature });

                ((LineSeries)chart.Series[0]).ItemsSource = eventCache.TakeLast(20);
            }
        }

        public class TimeValueItem
        {
            public string Time { get; set; }
            public double Value { get; set; }
        }


        private void UpTemp_Click(object sender, RoutedEventArgs e)
        {
            cs.TempOffset += 2;
        }

        private void DownTemp_Click(object sender, RoutedEventArgs e)
        {
            cs.TempOffset -= 2;
        }


        private void SubmitButton_Checked(object sender, RoutedEventArgs e)
        {
            if (deviceConnectionString.Text == "")
            {
                SubmitButton.IsChecked = false;
                SubmitButton.Focus(FocusState.Keyboard);
            }
            else
            {
                send = true;
                SubmitButton.Background = new SolidColorBrush(Windows.UI.Colors.Blue);
            }
        }

        private void SubmitButton_Unchecked(object sender, RoutedEventArgs e)
        {
            send = false;
            SubmitButton.Background = new SolidColorBrush(Windows.UI.Colors.DarkGray);
        }
    }

    public static class MiscExtensions
    {
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }
    }
}
