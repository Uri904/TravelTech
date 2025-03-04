using System;
using TravelTech;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage()); // Permite la navegación

            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
