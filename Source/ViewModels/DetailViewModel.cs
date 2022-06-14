using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Compass.ViewModels
{
    class DetailViewModel : INotifyPropertyChanged
    {
        public double Longitude { get { return longitude; } set { longitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude))); } }
        public double Latitude { get { return latitude; } set { latitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude))); } }
        public double Altitude { get { return altitude; } set { altitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Altitude))); } }
        public string LongitudeString { get { return longitudeString; } set { longitudeString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongitudeString))); } }
        public string LatitudeString { get { return latitudeString; } set { latitudeString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LatitudeString))); } }
        public string AltitudeString { get { return altitudeString; } set { altitudeString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AltitudeString))); } }
        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled))); } }
        public string HeadingString { get { return headingString; } set { headingString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingString))); } }
        public string DirectionString { get { return directionString; } set { directionString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectionString))); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isEnabled;
        private double longitude, latitude, altitude;
        private string longitudeString, latitudeString, altitudeString, headingString, directionString;
        private CancellationTokenSource cancellationToken;
        private CompassViewModel compassViewModel;

        private const double GPSUpdateIntervalMs = 5000;

        public DetailViewModel(CompassViewModel compassViewModel)
        {
            this.compassViewModel = compassViewModel;

            longitudeString = latitudeString = altitudeString = "-";
        }

        public void Enable()
        {
            isEnabled = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(GPSUpdateIntervalMs), () =>
            {
                UpdateGPSCoordinates();
                return isEnabled;
            });

            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                UpdateHeading();
                return isEnabled;
            });
        }

        public void Disable()
        {
            isEnabled = false;
        }

        private async void UpdateGPSCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cancellationToken = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cancellationToken.Token);

                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Altitude = location.Altitude ?? 0.0;

                    LatitudeString = (Latitude == 0) ? "-" : $"{Math.Abs(Latitude):F4}° {(Latitude >= 0 ? "N" : "S")}";
                    LongitudeString = (Longitude == 0) ? "-" : $"{Math.Abs(Longitude):F4}° {(Longitude >= 0 ? "E" : "W")}";
                    AltitudeString = (Altitude == 0) ? "-" : $"{Altitude:F2} m";
                }
            }
            catch { }
        }

        private void UpdateHeading()
        {
            HeadingString = compassViewModel.HeadingString;
            DirectionString = compassViewModel.DirectionString;
        }
    }
}
