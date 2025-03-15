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
    public partial class Crear : ContentPage
    {
        public Crear()
        {
            InitializeComponent();
        }

        // -- Navegación -- //

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton btn_AgregarDestino
        private async void btn_AgregarDestino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.Crear());
        }


        //Evento del boton btn_AgregarActividad
        private async void btn_AgregarActividad(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.CrearActividad());
        }




        }
}