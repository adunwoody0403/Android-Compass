using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Compass.ViewModels;

namespace Compass.Views
{
    public class CompassView : ContentView
    {
        public CompassViewModel ViewModel => viewModel;

        CompassViewModel viewModel;
        Image compassArrow, compassFace, compassBearing;
        const int lightGrayShade = 220;
        const int darkGrayShade = 35;

        public CompassView()
        {
            viewModel = new CompassViewModel();
            BindingContext = viewModel;
            Application.Current.RequestedThemeChanged += OnThemeChange;
            Content = InitializeCompassView();
        }

        private View InitializeCompassView()
        {
            //BackgroundColor = Color.FromRgb(lightGrayShade, lightGrayShade, lightGrayShade)
            var layout = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            // Compass bearing
            compassBearing = new Image();
            compassBearing.SetBinding(Image.RotationProperty, nameof(CompassViewModel.BearingRotation));
            compassBearing.SetBinding(Image.RotationXProperty, nameof(CompassViewModel.Roll));
            compassBearing.SetBinding(Image.RotationYProperty, nameof(CompassViewModel.Pitch));

            // Compass arrow
            compassArrow = new Image()
            {
                Source = "CompassNeedle.png"
            };
            compassArrow.SetBinding(Image.RotationXProperty, nameof(CompassViewModel.Roll));
            compassArrow.SetBinding(Image.RotationYProperty, nameof(CompassViewModel.Pitch));
            compassArrow.SetBinding(Image.RotationProperty, nameof(CompassViewModel.InverseHeading));

            // Compass face
            compassFace = new Image();
            compassFace.SetBinding(Image.RotationXProperty, nameof(CompassViewModel.Roll));
            compassFace.SetBinding(Image.RotationYProperty, nameof(CompassViewModel.Pitch));


            layout.Children.Add(compassBearing, new Rectangle(0.5, 0.5, 1.0, 1.0), AbsoluteLayoutFlags.All);
            layout.Children.Add(compassFace, new Rectangle(0.5, 0.5, 1.0, 1.0), AbsoluteLayoutFlags.All);
            layout.Children.Add(compassArrow, new Rectangle(0.5, 0.5, 256, 256), AbsoluteLayoutFlags.PositionProportional);

            OnThemeChange(null, null);
            return layout;
        }

        private void OnThemeChange(object sender, EventArgs args)
        {
            BackgroundColor = AppInfo.RequestedTheme == AppTheme.Light ? Color.FromRgb(lightGrayShade, lightGrayShade, lightGrayShade) : Color.FromRgb(darkGrayShade, darkGrayShade, darkGrayShade);
            compassFace.Source = AppInfo.RequestedTheme == AppTheme.Light ? "CompassFace_Light.png" : "CompassFace_Dark.png";
            compassBearing.Source = AppInfo.RequestedTheme == AppTheme.Light ? "CompassBearing_Light.png" : "CompassBearing_Dark.png";
        }
    }
}