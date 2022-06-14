using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Numerics;

namespace Compass.ViewModels
{
    public class CompassViewModel : INotifyPropertyChanged
    {
        public double Heading { get => heading; set { heading = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Heading))); } }
        public string HeadingString { get => headingString; set { headingString = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingString))); } }
        public double InverseHeading { get => inverseHeading; set { inverseHeading = value; PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(InverseHeading))); } }
        public double BearingRotation { get => bearingRotation; set { bearingRotation = value; PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(BearingRotation))); } }
        public string DirectionString {
            get
            {
                return directionString;
            }
            set 
            { 
                directionString = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectionString))); 
            }
        }
        public double Pitch { get => pitch; private set { pitch = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pitch))); } }
        public double Roll { get => roll; private set { roll = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Roll))); } }

        private double heading, pitch, roll, inverseHeading, bearingRotation;
        private string headingString, directionString;
        private CompassDirection direction;

        private const double headingLerpRate = 0.1;
        private const double bearingLerpRate = 0.05;
        private const double maxPitchRollAngle = 60.0;


        public event PropertyChangedEventHandler PropertyChanged;

        private enum CompassDirection { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest }


        public CompassViewModel()
        {
            EnableDeviceCompass();
            EnableOrientationSensor();
        }

        ~CompassViewModel()
        {
            DisableDeviceCompass();
            DisableOrientationSensor();
        }

        public void EnableDeviceCompass()
        {
            try
            {
                Xamarin.Essentials.Compass.Start(SensorSpeed.Game, true);
                Xamarin.Essentials.Compass.ReadingChanged += UpdateHeading;
            }
            catch { }
        }

        public void DisableDeviceCompass()
        {
            try
            {
                if (Xamarin.Essentials.Compass.IsMonitoring)
                {
                    Xamarin.Essentials.Compass.Stop();
                    Xamarin.Essentials.Compass.ReadingChanged -= UpdateHeading;
                }
            }
            catch { }

            Heading = 0.0f;
        }

        public void EnableOrientationSensor()
        {
            try
            {
                OrientationSensor.Start(SensorSpeed.Game);
                OrientationSensor.ReadingChanged += UpdateOrientation;
            }
            catch { }
        }

        public void DisableOrientationSensor()
        {
            try
            {
                if (OrientationSensor.IsMonitoring)
                {
                    OrientationSensor.Stop();
                    OrientationSensor.ReadingChanged -= UpdateOrientation;
                }
            }
            catch { }
        }

        private void UpdateHeading(object sender, CompassChangedEventArgs args)
        {
            var data = args.Reading;
            double newHeading = data.HeadingMagneticNorth;
            double currentHeading = Heading;
            double difference = newHeading - currentHeading;
            if (difference > 180) currentHeading += 360;
            else if (difference < -180) currentHeading -= 360;
            Heading = Lerp(currentHeading, newHeading, headingLerpRate);
            if (Math.Abs(Heading) < 0.001) Heading = 0;
            HeadingString = $"{Heading:F2}°";

            InverseHeading = -Heading;

            double newBearing = InverseHeading;
            double currentBearing = BearingRotation;
            difference = newBearing - currentBearing;
            if (difference > 180) currentBearing += 360;
            else if (difference < -180) currentBearing -= 360;
            BearingRotation = Lerp(currentBearing, newBearing, bearingLerpRate);

            UpdateDirection();
        }

        private void UpdateOrientation(object sender, OrientationSensorChangedEventArgs args)
        {
            var data = args.Reading;
            Quaternion orientation = data.Orientation;

            double pitch, yaw, roll;
            ToPitchYawRoll(orientation, out pitch, out yaw, out roll);

            if (roll > maxPitchRollAngle) roll = maxPitchRollAngle;
            else if (roll < -maxPitchRollAngle) roll = -maxPitchRollAngle;
            Roll = roll;

            if (pitch > maxPitchRollAngle) pitch = maxPitchRollAngle;
            else if (pitch < -maxPitchRollAngle) pitch = -maxPitchRollAngle;
            Pitch = -pitch;
        }

        private void UpdateDirection()
        {
            if (Heading >= 22.5 && Heading < 67.5)
            {
                direction = CompassDirection.NorthEast;
            }
            else if (Heading >= 67.5 && Heading < 112.5)
            {
                direction = CompassDirection.East;
            }
            else if (Heading >= 112.5 && Heading < 157.5)
            {
                direction = CompassDirection.SouthEast;
            }
            else if (Heading >= 157.5 && Heading < 202.5)
            {
                direction = CompassDirection.South;
            }
            else if (Heading >= 202.5 && Heading < 247.5)
            {
                direction = CompassDirection.SouthWest;
            }
            else if (Heading >= 247.5 && Heading < 292.5)
            {
                direction = CompassDirection.West;
            }
            else if (Heading >= 292.5 && Heading < 337.5)
            {
                direction = CompassDirection.NorthWest;
            }
            else
            {
                direction = CompassDirection.North;
            }

            switch (direction)
            {
                default:
                case CompassDirection.North:
                    DirectionString = "North";
                    break;

                case CompassDirection.NorthEast:
                    DirectionString = "North East";
                    break;

                case CompassDirection.East:
                    DirectionString = "East";
                    break;

                case CompassDirection.SouthEast:
                    DirectionString = "South East";
                    break;

                case CompassDirection.South:
                    DirectionString = "South";
                    break;

                case CompassDirection.SouthWest:
                    DirectionString = "South West";
                    break;

                case CompassDirection.West:
                    DirectionString = "West";
                    break;

                case CompassDirection.NorthWest:
                    DirectionString = "North West";
                    break;
            }
        }
        
        private double Lerp(double a, double b, double t)
        {
            if (t > 1) t = 1;
            else if (t < 0) t = 0;

            return a + (t * (b - a));
        }

        private double AngleBetweenVectors(Vector3 vector1, Vector3 vector2)
        {
            return Math.Acos(Vector3.Dot(vector1, vector2) / (vector1.Length() * vector2.Length()));
        }

        private void ToPitchYawRoll(Quaternion q, out double pitch, out double yaw, out double roll)
        {
            // roll (x-axis rotation)
            double sinr_cosp = 2 * (q.W * q.X + q.Y * q.Z);
            double cosr_cosp = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            roll = Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch (y-axis rotation)
            double sinp = 2 * (q.W * q.Y - q.Z * q.X);
            if (Math.Abs(sinp) >= 1)
                pitch = Math.Sign(sinp) * Math.PI / 2; // use 90 degrees if out of range
            else
                pitch = Math.Asin(sinp);

            // yaw (z-axis rotation)
            double siny_cosp = 2 * (q.W * q.Z + q.X * q.Y);
            double cosy_cosp = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            yaw = Math.Atan2(siny_cosp, cosy_cosp);

            pitch = ToDegrees(pitch);
            yaw = ToDegrees(yaw);
            roll = ToDegrees(roll);
        }

        private double ToDegrees(double angleRadians)
        {
            return 360.0 * (angleRadians / (Math.PI * 2));
        }
    }
}
