using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Essentials;
using Compass.ViewModels;

namespace Compass.Views
{
    public class MainPageView : ContentPage
    {
        private MainPageViewModel viewModel;
        private CompassView compass;
        private DetailView detail;
        private bool enableDetailView;

        public MainPageView()
        {
            viewModel = new MainPageViewModel();
            BindingContext = viewModel;
            Content = InitializeMainPageView();
        }

        private View InitializeMainPageView()
        {
            
            var layout = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Register tap gesture
            TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
            layout.GestureRecognizers.Add(tapRecognizer);
            tapRecognizer.Tapped += OnTap;

            // Setup compass view
            compass = new CompassView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true
            };

            // Setup detail view
            detail = new DetailView(compass.ViewModel)
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Opacity = 0,
                InputTransparent = true
            };
            
            layout.Children.Add(compass, new Rectangle(0.5, 0.5, 1, 1), AbsoluteLayoutFlags.All);
            layout.Children.Add(detail, new Rectangle(0.5, 0.5, 1, 1), AbsoluteLayoutFlags.All);

            return layout;
        }

        private void OnTap(object sender, EventArgs args)
        {
            if (enableDetailView) DisableDetailMode();
            else EnableDetailMode();

            try
            {
                HapticFeedback.Perform(HapticFeedbackType.Click);
            }
            catch { }
        }

        private void EnableDetailMode()
        {
            enableDetailView = true;
            detail.Enable();
        }

        private void DisableDetailMode()
        {
            enableDetailView = false;
            detail.Disable();
        }
    }
}