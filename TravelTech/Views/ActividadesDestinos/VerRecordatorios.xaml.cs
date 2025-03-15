using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelTech.Views.ActividadesDestinos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerRecordatorios : ContentPage
    {
        public VerRecordatorios()
        {
            InitializeComponent();
        }

        // -- Despliegue de pagina -- //
        private void ToggleRecordatorio1(object sender, EventArgs e)
        {
            contentRecordatorio1.IsVisible = !contentRecordatorio1.IsVisible;
        }

        private void ToggleRecordatorio2(object sender, EventArgs e)
        {
            contentRecordatorio2.IsVisible = !contentRecordatorio2.IsVisible;
        }


        // -- Navegación -- // 

        //Evento del boton btn_Regresar
        private async void btn_Regresar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();// Regresa a la página anterior
        }

        //Evento del boton BtnEditar
        private async void BtnEditar(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.ActividadesDestinos.ActualizarRecordatoios());
        }

        //Evento del boton btn_AgregarRecordatorio
        private async void btn_AgregarRecordatorio(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.ActividadesDestinos.CrearRecordatorio());
        }


    }
}