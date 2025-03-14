using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.Destinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerDestinos : ContentPage
    {
        public VerDestinos()
        {
            InitializeComponent();
        }
        
        
        // Evento del Boton Agregar Destino
        private async void btn_AgregarDestino(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Destinos.Crear());
        }


        // Evento del Boton Detalles
        private async void btn_Detalles(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Viajes.Detalles());
        }

        // Evento del Boton Actividades
        private async void btn_Actividades(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ActividadesDestinos.VerActividad());
        }

        // Evento del btn_Home
        private async void btn_Home(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }


        //Evento de los botones ToggleParis y ToggleMontmartre cada uno se encuentra en un STACKLAYOUD cada uno
        private void ToggleParis(object sender, EventArgs e)
        {
            contentParis.IsVisible = !contentParis.IsVisible;
            btnParis.Text = contentParis.IsVisible ? "París ▲" : "París ▼";
        }

        private void ToggleMontmartre(object sender, EventArgs e)
        {
            contentMontmartre.IsVisible = !contentMontmartre.IsVisible;
            btnMontmartre.Text = contentMontmartre.IsVisible ? "Recorrer Montmartre ▲" : "Recorrer Montmartre ▼";
        }
        private async void BtnEditar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.Destinos.ActualizarDestino());
        }


    }
}