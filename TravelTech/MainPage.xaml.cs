using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelTech.Views;
using TravelTech.Views.Viajes;
using Xamarin.Forms;

namespace TravelTech
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //Evento btn_VerDetalles
        private async void btn_VerDetalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Detalles());
        }

        //Evento btn_Calendario
        private async void btn_Calendario(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CalendarioIn());
        }

        //Evento btn_CrearViaje
        private async void btn_CrearViaje(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Crear());
        }
    }
}
