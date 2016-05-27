using GHIElectronics.UWP.Shields;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using static GHIElectronics.UWP.Shields.FEZHAT;

namespace CloudSensor
{
    public class CloudSensorHelper
    {
        //A device specific connectionstring that let's us create a DeviceClient object that can send messages to Azure IoT Hub.
        private string deviceConnectionString;

        public string DeviceConnectionString
        {
            get { return deviceConnectionString; }
            set { deviceConnectionString = value; }
        }

        private string status = "";

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public bool Online { get; set; }



        public CloudSensorHelper()
        {
            TempOffset = 0.0;
            Init();
        }

        private async void Init()
        {
            try
            {
                //Try to create the HAT and if it fails we assume it is absent or the UWP app is not running on W10 IoT Core (f.e. local deploy from VS)
                HAT = await FEZHAT.CreateAsync();
            }
            catch
            {
                hatAvailable = false;
            }

            if (HAT == null)
            {
                hatAvailable = false;

            }
            else
            {
                hatAvailable = true;
                HAT.DIO24On = true;
                HAT.D2.Color = FEZHAT.Color.Green;
                HAT.D3.Color = FEZHAT.Color.Green;
            }
        }

        public async Task SendTestMessage()
        {
            Sense();
            await SendMeasurement();
        }

        public async Task SendMeasurement()
        {
           await SendMessage("{\"time\": \"" + DateTime.UtcNow.ToString() + "\",\"temperature\": \"" + Temperature + "\",\"lumen\": \"" + Lumen + "\"}");
        }

        public void Sense()
        {
            if (hatAvailable)
            {
                Lumen = (int)(HAT.GetLightLevel() * 1000);
                Temperature = HAT.GetTemperature() + TempOffset;

                //Vizualize sensor values in case the device has no screen attached or NanoRDP session.
                HAT.D2.Color = ColorHelper.GetHATColor((int)((Temperature / 38) * 100), Colors.Red, Colors.Blue);
                HAT.D3.Color = ColorHelper.GetHATColor((int)((Lumen / 1000) * 100),Colors.Yellow,Colors.Orange);
            }
            else
            {
                var r = new Random();
                Lumen = r.Next(800, 1150); 
                Temperature = 20 + TempOffset + (r.NextDouble());
            }
        }


        private async Task SendMessage(string messagePayload)
        {
            //Create the client using the device specific connectionstring.
            var deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Http1);
            var message = new Message(Encoding.ASCII.GetBytes(messagePayload));

            try
            {
                await deviceClient.SendEventAsync(message);

                Online = true;
            }
            catch 
            {
                Online = false;
            }
        }

        #region Properties
        private FEZHAT _hat;
        public FEZHAT HAT
        {
            get { return _hat; }
            set { _hat = value; }
        }


        private bool hatAvailable = false;


        private bool _upPressed;

        public bool UpPressed
        {
            get { return _upPressed; }
            set { _upPressed = value; }
        }

        private bool _downPressed;

        public bool DownPressed
        {
            get { return _downPressed; }
            set { _downPressed = value; }
        }

        private double _tempOffset;

        public double TempOffset
        {
            get { return _tempOffset; }
            set { _tempOffset = value; }
        }

        private double _temperature;

        public double Temperature
        {
            get { return _temperature; }
            set { _temperature = value; }
        }

        private double _lumen;

        public double Lumen
        {
            get { return _lumen; }
            set { _lumen = value; }
        }

        #endregion
    }
}
