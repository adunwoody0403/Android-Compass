using Compass.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Compass.Views
{
    public class DetailView : ContentView
    {
        private DetailViewModel viewModel;
        private CompassViewModel compassViewModel;

        private const int rowMargin = 50;
        private const int fontSize = 14;
        private LayoutOptions labelLayoutOption = LayoutOptions.StartAndExpand;
        private TextAlignment labelTextAlignment = TextAlignment.Start;
        private LayoutOptions valueLayoutOption = LayoutOptions.StartAndExpand;
        private TextAlignment valueTextAlignment = TextAlignment.Start;

        public DetailView(CompassViewModel compassViewModel)
        {
            this.compassViewModel = compassViewModel;
            viewModel = new DetailViewModel(compassViewModel);
            Content = InitializeDetailView();
        }

        public void Enable()
        {
            this.FadeTo(1, 250, Easing.CubicInOut);
            viewModel.Enable();
        }

        public void Disable()
        {
            this.FadeTo(0, 250, Easing.CubicInOut);
            viewModel.Disable();
        }

        private View InitializeDetailView()
        {
            var layout = new AbsoluteLayout()
            {
                InputTransparent = true
            };

            var headingDetails = InitializeHeadingView();
            var gpsDetails = InitializeGpsDetailView();

            layout.Children.Add(headingDetails, new Rectangle(0.5, 0.1, 0.6, 0.2), AbsoluteLayoutFlags.All);
            layout.Children.Add(gpsDetails, new Rectangle(0.5, 1, 0.6, 0.2), AbsoluteLayoutFlags.All);

            return layout;
        }

        private View InitializeHeadingView()
        {
            BindingContext = compassViewModel;
            Frame headingFrame = new Frame()
            {
                InputTransparent = true,
                BackgroundColor = Color.FromRgba(0, 0, 0, 0)
            };

            StackLayout directionStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            Label directionLabel = new Label()
            {
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            BindingContext = viewModel;
            directionLabel.SetBinding(Label.TextProperty, nameof(DetailViewModel.DirectionString));
            directionLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));

            Label headingLabel = new Label()
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center
            };
            BindingContext = compassViewModel;
            headingLabel.SetBinding(Label.TextProperty, nameof(CompassViewModel.HeadingString));
            headingLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));

            directionStack.Children.Add(directionLabel);
            directionStack.Children.Add(headingLabel);
            
            headingFrame.Content = directionStack;

            return headingFrame;
        }

        private View InitializeGpsDetailView()
        {
            Frame gpsFrame = new Frame()
            {
                CornerRadius = 5,
                HasShadow = false,
                BackgroundColor = Color.FromRgba(0, 0, 0, 0),
                InputTransparent = true
            };
            //frame.SetAppThemeColor(Frame.BackgroundColorProperty, Color.FromRgba(0.1, 0.1, 0.1, 0.2), Color.FromRgba(1.0, 1.0, 1.0, 0.2));

            Grid gpsGrid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) }
                },

                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }
                },

                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            var headingLabel = new Label()
            {
                Text = "GPS",
                FontSize = fontSize,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = labelTextAlignment,
                HorizontalOptions = labelLayoutOption,
                //TextColor = Color.LightGray
            };
            //headingLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            headingLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));

            var gpsCoordinateLatRowLabel = new Label()
            {
                Text = "Latitude",
                FontSize = fontSize,
                HorizontalTextAlignment = labelTextAlignment,
                HorizontalOptions = valueLayoutOption
                //TextColor = Color.LightGray
            };
            //gpsCoordinateLatRowLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            gpsCoordinateLatRowLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));

            var gpsCoordinateLatLabel = new Label()
            {
                Text = "-",
                FontSize = fontSize,
                Margin = new Thickness(rowMargin, 0, 0, 0),
                HorizontalTextAlignment = valueTextAlignment,
                HorizontalOptions = valueLayoutOption
                //TextColor = Color.LightGray
            };
            BindingContext = viewModel;
            gpsCoordinateLatLabel.SetBinding(Label.TextProperty, nameof(DetailViewModel.LatitudeString));
            //gpsCoordinateLatLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            gpsCoordinateLatLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));


            var gpsCoordinateLongRowLabel = new Label()
            {
                Text = "Longitude",
                FontSize = fontSize,
                HorizontalTextAlignment = labelTextAlignment,
                HorizontalOptions = labelLayoutOption
                //TextColor = Color.LightGray
            };
            //gpsCoordinateLongRowLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            gpsCoordinateLongRowLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));

            var gpsCoordinateLongLabel = new Label()
            {
                Text = "-",
                FontSize = fontSize,
                Margin = new Thickness(rowMargin, 0, 0, 0),
                HorizontalOptions = valueLayoutOption,
                HorizontalTextAlignment = valueTextAlignment,
                //TextColor = Color.LightGray
            };
            gpsCoordinateLongLabel.SetBinding(Label.TextProperty, nameof(DetailViewModel.LongitudeString));
            //gpsCoordinateLongLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            gpsCoordinateLongLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));

            var gpsCoordinateAltRowLabel = new Label()
            {
                Text = "Altitude",
                FontSize = fontSize,
                HorizontalTextAlignment = labelTextAlignment,
                HorizontalOptions = labelLayoutOption
                //TextColor = Color.LightGray
            };
            //gpsCoordinateAltRowLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            gpsCoordinateAltRowLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));


            var gpsCoordinateAltLabel = new Label()
            {
                Text = "-",
                FontSize = fontSize,
                Margin = new Thickness(rowMargin, 0, 0, 0),
                HorizontalOptions = valueLayoutOption,
                HorizontalTextAlignment = valueTextAlignment,
                //TextColor = Color.LightGray
            };
            gpsCoordinateAltLabel.SetBinding(Label.TextProperty, nameof(DetailViewModel.AltitudeString));
            //gpsCoordinateAltLabel.SetAppThemeColor(Label.TextColorProperty, Color.DarkGray, Color.White);
            gpsCoordinateAltLabel.SetAppThemeColor(Label.TextColorProperty, Color.FromRgb(35, 35, 35), Color.FromRgb(220, 220, 220));


            gpsGrid.Children.Add(headingLabel, 0, 0);
            gpsGrid.Children.Add(gpsCoordinateLatRowLabel, 0, 1);
            gpsGrid.Children.Add(gpsCoordinateLatLabel, 1, 1);
            gpsGrid.Children.Add(gpsCoordinateLongRowLabel, 0, 2);
            gpsGrid.Children.Add(gpsCoordinateLongLabel, 1, 2);
            gpsGrid.Children.Add(gpsCoordinateAltRowLabel, 0, 3);
            gpsGrid.Children.Add(gpsCoordinateAltLabel, 1, 3);

            gpsFrame.Content = gpsGrid;

            return gpsFrame;
        }
    }
}