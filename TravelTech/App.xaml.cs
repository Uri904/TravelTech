using Xamarin.Forms;
using TravelTech.Views;

namespace TravelTech
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Crear el FlyoutPage
            var flyout = new FlyoutPage();

            // Configurar la página del menú (Master)
            flyout.Flyout = new MenuLateral();

            // Configurar la página principal (Detail)
            flyout.Detail = new NavigationPage(new MainPage());

            // Establecer como página principal
            MainPage = flyout;
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