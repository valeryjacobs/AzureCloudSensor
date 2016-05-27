
using GHIElectronics.UWP.Shields;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;


namespace CloudSensor
{
    public class CloudSensorHelper
    {

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


        //SNIP1 (remove the Init() method first)
        private async void Init()
        {

        }

       

        public void Sense()
        {
            if (hatAvailable)
            {
                //SNIP2
            }
            else
            {
                var r = new Random();
                Lumen = r.Next(800, 1150);
                Temperature = 20 + TempOffset + (r.NextDouble());
            }
        }

        //SNIP3


        #region Properties

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
