using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Viajes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detalles : ContentPage
    {
        public Detalles()
        {
            InitializeComponent();
        }


        // -- Navegación -- //

        //Evento btn_Home
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        //Evento btn_Destino
        private async void btn_Destino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.VerDestinos());
        }

        //Evento btn_Actividades
        private async void btn_Actividades(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.VerActividad());
        }

        //Evento btn_Gastos
        private async void btn_Gastos(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Gastos.VerGastos());
        }

        //Evento btn_EditarDetalles
        private async void btn_EditarDetalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.EditarDetalles());
        }






    }
}